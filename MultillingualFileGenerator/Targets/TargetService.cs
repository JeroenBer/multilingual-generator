using MultillingualFileGenerator.Config;
using MultillingualFileGenerator.Sources.Model;
using MultillingualFileGenerator.Targets.Model;
using MultillingualFileGenerator.Util;
using MultillingualFileGenerator.Xliff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Targets
{
    public class TargetService
    {
        private readonly XliffReader _xliffReader;
        private readonly XliffWriter _xliffWriter;
        private readonly TargetWriterFactory _targetWriterFactory;

        public TargetService(XliffReader xliffReader, XliffWriter xliffWriter, TargetWriterFactory targetWriterFactory)
        {
            _xliffReader = xliffReader;
            _xliffWriter = xliffWriter;
            _targetWriterFactory = targetWriterFactory;
        }

        public void ProcessTarget(SourceInput sourceInput, TargetSettings targetSettings, Target target, string workingDir)
        {
            Console.Write($"Processing target \"{target.TargetLanguage}\": ");

            var xliffFilePath = Path.Combine(workingDir, targetSettings.XliffBaseDir, target.TargetXliff);

            // Read existing Xliff if present
            IndexedList<XliffTransUnitElement> indexedXliffTransUnitElements;
            if (File.Exists(xliffFilePath))
            {
                indexedXliffTransUnitElements = _xliffReader.Read(xliffFilePath);
            }
            else
            {
                indexedXliffTransUnitElements = new IndexedList<XliffTransUnitElement>(e => e.ID);
            }

            // Update existing Xliff with new source input
            var hasChanges = UpdateXliffTransUnitElements(indexedXliffTransUnitElements, sourceInput);

            // Write new Xliff
            var newXliffFile = CreateUpdatedXliffFile(sourceInput, indexedXliffTransUnitElements, target);

            Directory.CreateDirectory(Path.GetDirectoryName(xliffFilePath));   // Create directory if necessary
            _xliffWriter.Write(xliffFilePath, newXliffFile);

            // Write new target output based on Xliff
            var targetLines = indexedXliffTransUnitElements
                .Where(e => e.Target.State != TranslationStates.New)
                .Select(e => new TargetLine()
                {
                    Name = e.ID,
                    Value = e.Target.Value,
                }).ToList();

            var targetFilePath = Path.Combine(workingDir, targetSettings.ResourcesBaseDir, target.TargetResource);
            var targetWriter = _targetWriterFactory.GetTargetWriter(targetSettings.TargetFileFormat);

            Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));   // Create directory if necessary
            targetWriter.Write(targetFilePath, targetLines);

            int countNew = indexedXliffTransUnitElements.Count(e => e.Target?.State == TranslationStates.New);
            int countNeedsReview = indexedXliffTransUnitElements.Count((e => e.Target?.State == TranslationStates.NeedsReview));
            int countTotal = indexedXliffTransUnitElements.Count();

            Console.WriteLine($"New={countNew}, Needs review={countNeedsReview}, Total={countTotal}");
        }

        private bool UpdateXliffTransUnitElements(IndexedList<XliffTransUnitElement> indexedXliffTransUnitElements, SourceInput sourceInput)
        {
            var hasChanges = false;
            // always keep the order in the Xliff as leading

            // Delete elements that are not longer present
            foreach (var transUnitElement in indexedXliffTransUnitElements.ToList())
            {
                if (!sourceInput.Lines.ContainsKey(transUnitElement.ID))
                {
                    indexedXliffTransUnitElements.Remove(transUnitElement.ID);
                    hasChanges = true;
                }
            }

            // Now update or add new elements by going through the source list
            foreach (var sourceLine in sourceInput.Lines)
            {
                if (indexedXliffTransUnitElements.ContainsKey(sourceLine.Name))
                {
                    var transUnitElement = indexedXliffTransUnitElements[sourceLine.Name];

                    // Update if value changed
                    if ((sourceLine.Value ?? string.Empty) != (transUnitElement.Source.Value ?? string.Empty))
                    {
                        transUnitElement.Source.Value = sourceLine.Value;
                        if (transUnitElement.Target.State != TranslationStates.New)
                        {
                            transUnitElement.Target.State = TranslationStates.NeedsReview;
                            transUnitElement.Note = new XliffNoteElement
                            {
                                // <note from="MultilingualUpdate" annotates="source" priority="2">Please verify the translation’s accuracy as the source string was updated after it was translated.</note>
                                From = "MultilingualUpdate",
                                Annotates = "source",
                                Priority = "2",
                                Value = "Please verify the translation’s accuracy as the source string was updated after it was translated.",
                            };
                        }
                        hasChanges = true;
                    }
                    else
                    {
                        transUnitElement.Source.Value ??= string.Empty; // If null then put in empty string
                    }
                }
                else
                {
                    // Add
                    indexedXliffTransUnitElements.Add(new XliffTransUnitElement
                    {
                        ID = sourceLine.Name,
                        Translate = "yes",
                        Space = "preserve",
                        Source = new XliffSourceElement
                        {
                            Value = sourceLine.Value,
                        },
                        Target = new XliffTargetElement
                        {
                            State = TranslationStates.New,
                            Value = sourceLine.Value ?? string.Empty,
                        }
                    });
                    hasChanges = true;
                }
            }
            return hasChanges;
        }

        private XliffFile CreateUpdatedXliffFile(SourceInput sourceInput, IndexedList<XliffTransUnitElement> indexedXliffTransUnitElements, Target target)
        {
            var xliffFile = new XliffFile
            {
                Version = "1.2",
                XsiSchemaLocation = "urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd",
                File =
                [
                    new XliffFileElement
                    {
                        DataType = "xml",
                        SourceLanguage = sourceInput.SourceLanguage,
                        TargetLanguage = target.TargetLanguage,
                        Original = $"{sourceInput.ApplicationName?.ToUpperInvariant()}/{sourceInput.RelativeSourcePath.ToUpperInvariant()}",
                        ToolID="MultilingualFileGenerator",
                        ProductName="n/a",
                        ProductVersion="n/a",
                        BuildNum="n/a",
                        Header = new XliffHeaderElement
                        {
                            Tool = new XliffToolElement
                            {
                                ToolID = "MultilingualFileGenerator",
                                ToolName = "Multilingual File Generator",
                                ToolCompany = "Open Source",
                                ToolVersion = ToolVersion.GetVersion(),
                            },
                        },
                        Body = new XliffBodyElement
                        {
                            Group = new XliffGroupElement
                            {
                                ID = $"{sourceInput.ApplicationName?.ToUpperInvariant()}/{sourceInput.RelativeSourcePath.ToUpperInvariant()}",
                                DataType = "resx",
                                TransUnits = indexedXliffTransUnitElements.ToArray()
                            },
                        }
                    }
                ]
            };

            return xliffFile;
        }
    }
}
