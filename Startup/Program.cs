using System.CommandLine;
using Startup;

// migration creation requires us to at least start a host builder
// but we're trying to use a command pattern
// we just check if this is a entity framework's invocation,
// and simply start the server that it so desires.
if (args[0] == "--applicationName")
{
    Cli.StartServer(args);
}
var rootCommand = new RootCommand("key management service");

var serverSubcommand = new Command("server", "manage server");

var startServerSubCommand = new Command("start", "start the server");

startServerSubCommand.SetHandler(() => { Cli.StartServer(args); });

serverSubcommand.AddCommand(startServerSubCommand);
rootCommand.AddCommand(serverSubcommand);

var dbCommand = new Command("db", "manage database");
var dbMigrateCommand = new Command("migrate", "run migrations");
dbMigrateCommand.SetHandler(() => { Cli.Migrate(args); });

dbCommand.AddCommand(dbMigrateCommand);
rootCommand.AddCommand(dbCommand);

rootCommand.InvokeAsync(args);
