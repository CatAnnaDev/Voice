using System;
using System.Speech.Recognition;

#pragma warning disable CA1416
namespace Voice {
    public class Program : IDisposable {
        private static SpeechRecognitionEngine reco;
        static void Main(string[] args) {
            bool Reject = false;
            bool Detect = false;
            bool log = true;
            string Lan = "fr-FR";

            Console.WriteLine($"0 For DictationGrammar\n1 For GrammarBuilder \n2 For KeywordRecognizer");
            string choise = Console.ReadLine(); ;
            switch(choise) {
                case "0":
                Console.WriteLine($"DictationGrammar");
                try {
                    using(reco = new SpeechRecognitionEngine(new System.Globalization.CultureInfo(Lan))) {
                        reco.LoadGrammar(new DictationGrammar());
                        if(log)
                            reco.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(reco_SpeechReco);
                        //reco.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(reco_AudioLevelUpdated);
                        if(Detect)
                            reco.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(reco_SpeechDetected);
                        if(Reject)
                            reco.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(reco_SpeechReject);

                        reco.SetInputToDefaultAudioDevice();
                        reco.RecognizeAsync(RecognizeMode.Multiple);
                        Console.WriteLine($"Starting asynchronous recognition...\nLanguage: {Lan}\nSpeechRecognized: {log}\nSpeechDetected: {Detect}\nSpeechRecognitionRejected: {Reject}\n\n You can speeak now! ");
                        Console.ReadKey();
                    }
                }
                catch(Exception ex) {
                    reco.AudioSignalProblemOccurred += new EventHandler<AudioSignalProblemOccurredEventArgs>(sre_AudioSignalProblemOccurred);
                    static void sre_AudioSignalProblemOccurred(object sender, AudioSignalProblemOccurredEventArgs e) {

                        Console.WriteLine("Audio signal problem information:");
                        Console.WriteLine(
                          " Audio level:               {0}" + Environment.NewLine +
                          " Audio position:            {1}" + Environment.NewLine +
                          " Audio signal problem:      {2}" + Environment.NewLine +
                          e.AudioLevel, e.AudioPosition, e.AudioSignalProblem);
                    }
                    Console.WriteLine(ex.ToString());
                }
                break;
                case "1":
                Console.WriteLine($"GrammarBuilder");
                break;
                case "2":
                Console.WriteLine($"KeywordRecognizer");
                break;
            }
        }
        public static void reco_SpeechDetected(object sender, SpeechDetectedEventArgs e) {
            Console.WriteLine();
            Console.WriteLine("Speech detected:");
            Console.WriteLine("  Audio position at the event: " + e.AudioPosition);
            Console.WriteLine("  Current audio position: " + e.AudioPosition);
        }
        public static void reco_SpeechReject(object sender, SpeechRecognitionRejectedEventArgs e) {
            Console.WriteLine("Speech input was rejected.");
            foreach(RecognizedPhrase phrase in e.Result.Alternates) {
                Console.WriteLine("  Rejected phrase: " + phrase.Text);
                Console.WriteLine("  Confidence score: " + phrase.Confidence);
            }
        }
        public static void reco_SpeechReco(object sender, SpeechRecognizedEventArgs e) {
            Console.WriteLine($"Reco: {e.Result.Text}");
            switch(e.Result.Text.ToLower()) {
                case "salut":
                Console.WriteLine("Bonjour");
                break;
                case "stop":
                Environment.Exit(0);
                break;
            }
        }
        public void Dispose() {
            reco.RecognizeAsyncStop();
        }
    }
}
