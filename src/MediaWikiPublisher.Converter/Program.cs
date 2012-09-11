using MediaWikiPublisher.Converter.Model.Import;
using MediaWikiPublisher.Converter.Publishing;
using NLog;
using NLog.Config;
using NLog.Layouts;

namespace MediaWikiPublisher.Converter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new LoggingConfiguration();
            var target = new NLog.Targets.ColoredConsoleTarget { Layout = new SimpleLayout("${message}") };
            configuration.AddTarget("ColoredConsoleTarget", target);
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = configuration;

            var log = LogManager.GetLogger("Program");


            log.Info("Loading xml dump...");
            var content = WikiImporter.Import("pages.xml");

            log.Info("Compiling and publishing...");
            var publisher = new EpubPublisher();
            publisher.Publish(content, string.Format("{0}.epub", content.Title));

//            var parser = new WikiMarkupParser();
//            const string text = @"== Mass Effect ==
//=== Paragon ===
//[[Image:ME1 Paragon.png|right|150px]]
//Paragon points are gained for compassionate and heroic actions. The Paragon measurement is colored blue. Points are often gained when asking about feelings and motivations of characters. 
//Paragon or Charm dialogue choices (colored blue in dialogue trees) often lead to people being more open and friendly with Shepard, 
//and can sometimes avert entire battles that would otherwise be triggered.
//
//Shepard starts the game with 3 open Charm ranks. The Paragon scale affects Charm and more as defined below:
//:10% – Opens 2 Charm ranks. Gives 1 Charm point.
//:25% – Opens 2 Charm ranks. Gives 1 Charm point. Bonus: 10% shorter [[First Aid]] cooldown.
//:50% – No charm ranks/points. Bonus: 10% maximum [[health]].
//:75% – Opens 2 Charm ranks. Gives 1 Charm point. Bonus: 5% reduction in cooldown on all powers. [[Achievement]].
//
//After reaching 80% Paragon points, [[Admiral Hackett]] will give Shepard the [[UNC: Besieged Base]] assignment.
//
//The ""free"" bonus skill points can be gained again if importing the character for another playthrough.
//
//=== Renegade ===
//[[Image:ME1 Renegade.png|right|150px]]
//Renegade points are gained for apathetic and ruthless actions. The Renegade measurement is colored red. Many sarcastic and joking remarks are assigned Renegade points. 
//Renegade or Intimidate dialogue choices (colored red in dialogue trees) generally lead to people disliking and even fearing Shepard,
//and occasionally ""encourage"" people to tell or give more than they otherwise would. Like with Paragon/Charm options, Shepard can sometimes avert entire battles that would otherwise be triggered.
//
//Shepard starts with 3 open Intimidate ranks. The Renegade scale affects Intimidate and more as defined below:
//:10% – Opens 2 Intimidate ranks, gives 1 Intimidate point.
//:25% – Opens 2 Intimidate ranks, gives 1 Intimidate point, 10% reduction in weapon powers cooldown.
//:50% – 1 health regeneration per second.
//:75% – Achievement, opens 2 Intimidate ranks, gives 1 Intimidate point, 5% increase in damage/duration on all weapons and powers
//
//After reaching 80% Renegade points, [[Admiral Hackett]] will give Shepard the [[UNC: The Negotiation]] assignment.
//
//The ""free"" bonus skill points can be gained again if importing the character for another playthrough.
//{{Spoilers (Mass Effect)}}
//
//=== Spectre Bonus ===
//Becoming a [[Spectre]] in Mass Effect opens 3 more ranks and gives 1 skill point in both Charm and Intimidate.";
//            var node = parser.Parse("Paragon", text);

//            var compiler = new HtmlCompiler(new HtmlResourceManagerStub(), new CssStyleManagerStub());

//            var doc = compiler.Compile(node);
//            Console.WriteLine(doc.ToString());

//            Console.ReadKey();
        }
    }
}
