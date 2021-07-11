using System;
using System.Speech.Recognition;

#pragma warning disable CA1416
namespace Voice {
    public class Program : IDisposable {
        private static SpeechRecognitionEngine reco;
        static void Main(string[] args) {
            bool Reject = false;
            bool Detect = false;
            using(reco = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("fr-FR"))) {
                reco.LoadGrammar(new DictationGrammar());
                reco.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(reco_SpeechReco);
                if(Detect)
                    reco.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(reco_SpeechDetected);
                if(Reject)
                    reco.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(reco_SpeechReject);

                reco.SetInputToDefaultAudioDevice();
                reco.RecognizeAsync(RecognizeMode.Multiple);
                Console.WriteLine("Starting asynchronous recognition...");
                while(true) {
                    Console.ReadKey();
                }
            }
        }
        static void reco_SpeechDetected(object sender, SpeechDetectedEventArgs e) {
            Console.WriteLine();
            Console.WriteLine("Speech detected:");
            Console.WriteLine("  Audio level: " + reco.AudioLevel);
            Console.WriteLine("  Audio position at the event: " + e.AudioPosition);
            Console.WriteLine("  Current audio position: " + reco.AudioPosition);
            Console.WriteLine("  Current recognizer audio position: " + reco.RecognizerAudioPosition);
        }
        static void reco_SpeechReject(object sender, SpeechRecognitionRejectedEventArgs e) {
            Console.WriteLine("Speech input was rejected.");
            foreach(RecognizedPhrase phrase in e.Result.Alternates) {
                Console.WriteLine("  Rejected phrase: " + phrase.Text);
                Console.WriteLine("  Confidence score: " + phrase.Confidence);
            }
        }
        static void reco_SpeechReco(object sender, SpeechRecognizedEventArgs e) {
            bool log = true;
            if(log) {
                Console.WriteLine($"Reco: {e.Result.Text}");
            }

            switch(e.Result.Text.ToLower()) {
                case "salut":
                Console.WriteLine("Bonjour");
                break;
            }
        }
        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}
