using System.CommandLine;
using Startup;

// migration creation requires us to at least start a host builder
// but we're trying to use a command pattern
// we just check if this is a entity framework's invocation,
// and simply start the server that it so desires.
if (args[0] == "--applicationName") Cli.StartServer(args);

var rootCommand = new RootCommand("key management service");

var serverSubcommand = new Command("server", "manage server");

var startServerSubCommand = new Command("start", "start the server");

startServerSubCommand.SetHandler(() => { Cli.StartServer(args); });

serverSubcommand.AddCommand(startServerSubCommand);
rootCommand.AddCommand(serverSubcommand);

var dbCommand = new Command("db", "manage database");
var dbMigrateCommand = new Command("migrate", "run migrations");
dbMigrateCommand.SetHandler(() => { Cli.Migrate(args); });
var dbArchiveCommand = new Command("archive_keys", "archive old keys");
var keyTtlHours = new Option<int>("--key-ttl-hours", () => 72);
dbArchiveCommand.AddOption(keyTtlHours);
dbArchiveCommand.SetHandler(hoursBefore => { Cli.CleanupKeys(args, hoursBefore); }, keyTtlHours);
dbCommand.AddCommand(dbArchiveCommand);

dbCommand.AddCommand(dbMigrateCommand);
rootCommand.AddCommand(dbCommand);

return await rootCommand.InvokeAsync(args);
