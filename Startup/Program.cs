using System.CommandLine;
using Startup;

var rootCommand = new RootCommand("GetWeed backend subgraph component");

var serverSubcommand = new Command("server", "manage server");

var startServerSubCommand = new Command("start", "start the server");

startServerSubCommand.SetHandler(() =>
{
    Cli.StartServer(args);
});

serverSubcommand.AddCommand(startServerSubCommand);
rootCommand.AddCommand(serverSubcommand);

var dbCommand = new Command("db", "manage database");
var dbMigrateCommand = new Command("migrate", "run migrations");
dbMigrateCommand.SetHandler(() =>
{
    Cli.Migrate(args);
});

dbCommand.AddCommand(dbMigrateCommand);
rootCommand.AddCommand(dbCommand);

rootCommand.InvokeAsync(args);
