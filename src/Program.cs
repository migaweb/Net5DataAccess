using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace code
{
    class Program
    {
         static void Main(string[] args)
        {
            string key = string.Empty;
            try
            {   
                key = args.FirstOrDefault(x => x.Contains(".csproj") == false);
                s_runner[key].Invoke();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            System.Console.WriteLine($"ran with key: {key}");
            Thread.Sleep(500);
        }

         static Program()
        {
            s_runner = new Dictionary<string, Action>
            {
                ["ado.connect"] = ado.connect.Connecting.Run,
                ["ado.reader"] = ado.readers.DataReaders.Run,
                ["ado.params"] = ado.parameters.Param.Run,
                ["ado.cmds"] = ado.commands.Scalars.Run,
                ["ado.adapter"] = ado.adapter.DataAdapters.Run,
                ["ado.adapter.cmds"] = ado.adapter.Commands.Run,
                ["ado.transaction"] = ado.transaction.Acid.Run,

                ["ef.commands"] = ef.Commands.Run,
                ["dapper.queries"] = dapper.Queries.Run,
                ["dapper.commands"] = dapper.Commands.Run,

                ["dapper.parameters"] = dapper.Parameters.Run,
                ["dapper.qversions"] = dapper.QueryVersions.Run,
                ["dapper.dynamic"] = dapper.UseDynamicParams.Run,
                ["dapper.multiple"] = dapper.MultipleResults.Run,

                ["nosql.mongo"] = nosql.Mongo.Run,
                ["nosql.redis"] = nosql.redis.Commands.Run
            };
        }

        private static Dictionary<string, Action> s_runner;
    }
}
