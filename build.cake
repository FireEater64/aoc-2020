Task("Run all")
    .DoesForEach(GetFiles("./src/**/day*.csproj").OrderBy(x => x.ToString()), file =>
    {
        var taskName = $"{file.GetFilenameWithoutExtension()}";

        Task(taskName)
            .Does(c =>
            {
                Context.Environment.WorkingDirectory = file.GetDirectory();
                DotNetCoreRun(file.ToString());
            });

        RunTarget(taskName);
    });

RunTarget("Run all");
