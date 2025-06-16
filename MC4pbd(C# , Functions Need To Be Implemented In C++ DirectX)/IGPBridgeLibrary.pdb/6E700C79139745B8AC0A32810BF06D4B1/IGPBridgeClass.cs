// 
// Current member / type: IGPBridgeLibrary.IGPBridgeClass
// File path: C:\Users\Atraills\AppData\Local\Temp\JustDecompile\Sources\IGPBridgeLibrary.pdb\6E700C79139745B8AC0A32810BF06D4B1\IGPBridgeClass.cs
// 
// Product version: 2024.1.131.0
// Format of the executable (.exe) or library (.dll) is invalid.
//    at Mono.Cecil.PE.ImageReader.ReadOptionalHeaders(UInt16& subsystem, UInt16& dll_characteristics) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil.PE\ImageReader.cs:line 202
//    at Mono.Cecil.PE.ImageReader.ReadImage() in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil.PE\ImageReader.cs:line 87
//    at Mono.Cecil.PE.ImageReader.ReadImageFrom(Stream stream) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil.PE\ImageReader.cs:line 662
//    at Mono.Cecil.ModuleDefinition.ReadModule(Stream stream, ReaderParameters parameters) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\ModuleDefinition.cs:line 1049
//    at Mono.Cecil.ModuleDefinition.ReadModule(String fileName, ReaderParameters parameters) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\ModuleDefinition.cs:line 1033
//    at Mono.Cecil.AssemblyResolver.AssemblyPathResolver.TryGetAssemblyNameDefinition(String assemblyFilePath, Boolean caching, TargetArchitecture architecture, AssemblyName& assemblyName, Boolean checkForArchitectPlatfrom) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil.AssemblyResolver\AssemblyPathResolver.cs:line 489
//    at Mono.Cecil.AssemblyResolver.AssemblyPathResolver.CheckFileExistence(AssemblyName assemblyName, String searchPattern, Boolean caching, Boolean checkForBaseDir, Boolean checkForArchitectPlatfrom) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil.AssemblyResolver\AssemblyPathResolver.cs:line 86
//    at Mono.Cecil.BaseAssemblyResolver.TrySearchDirectory(AssemblyNameReference name, ReaderParameters parameters, TargetArchitecture architecture, IEnumerable`1 targetDirs, AssemblyDefinition& assemblyDefinition) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 500
//    at Mono.Cecil.BaseAssemblyResolver.<>c__DisplayClass39_0.<SearchDirectory>b__0() in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 473
//    at Mono.Cecil.BaseAssemblyResolver.DoWithReadLock[T](ReaderWriterLockSlim locker, Func`1 func) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 1106
//    at Mono.Cecil.BaseAssemblyResolver.SearchDirectory(AssemblyNameReference name, ReaderParameters parameters, TargetArchitecture architecture, String defaultPath) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 467
//    at Mono.Cecil.BaseAssemblyResolver.<>c__DisplayClass34_0.<Resolve>b__1() in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 355
//    at Mono.Cecil.BaseAssemblyResolver.DoWithWriteLock[T](ReaderWriterLockSlim locker, Func`1 func) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 1134
//    at Mono.Cecil.BaseAssemblyResolver.Resolve(AssemblyNameReference name, String defaultPath, ReaderParameters parameters, TargetArchitecture architecture, SpecialTypeAssembly special, Boolean bubbleToUserIfFailed, Boolean addToFailedCache) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\BaseAssemblyResolver.cs:line 344
//    at ..Resolve(AssemblyNameReference , String , TargetArchitecture , SpecialTypeAssembly , Boolean ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\WeakAssemblyResolver.cs:line 22
//    at Mono.Cecil.MetadataResolver.Resolve(TypeReference type, ICollection`1 visitedDlls) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\MetadataResolver.cs:line 156
//    at Mono.Cecil.MetadataResolver.Resolve(TypeReference type) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\MetadataResolver.cs:line 127
//    at Mono.Cecil.TypeReference.Resolve() in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Mono.Cecil\Mono.Cecil\TypeReference.cs:line 345
//    at ..GetExpandedTypeDependanceList(HashSet`1 ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Utilities.cs:line 394
//    at ..(ModuleDefinition ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\TypeCollisionWriterContextService.cs:line 194
//    at ..(ModuleDefinition , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\TypeCollisionWriterContextService.cs:line 310
//    at ..(ModuleDefinition , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\TypeCollisionWriterContextService.cs:line 59
//    at ..(TypeDefinition , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\TypeCollisionWriterContextService.cs:line 117
//    at ..(IMemberDefinition , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\TypeCollisionWriterContextService.cs:line 25
//    at ..(TypeDefinition ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Languages\NamespaceImperativeLanguageWriter.cs:line 35
//    at JustDecompile.Tools.MSBuildProjectBuilder.BaseProjectBuilder.WriteTypeToFile(TypeDefinition type,  itemWriter, Dictionary`2 membersToSkip, Boolean shouldBePartial, ILanguage language, List`1& writingInfos, String& theCodeString) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\MSBuildProjectCreator\BaseProjectBuilder.cs:line 925
