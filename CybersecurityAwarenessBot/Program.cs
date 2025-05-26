// Importing necessary libraries
using System;
using System.Collections.Generic;
using System.Media;
using System.IO;
using System.Threading;
using System.Speech;
using System.Speech.Synthesis;
using Microsoft.Office.Interop.Excel;
using System.Linq;

namespace CybersecurityAwarenessBot
{
    // Delegate for handling responses based on sentiment
    public delegate string ResponseHandler(string input, string userName, Sentiment sentiment);

    // Enum for different sentiment types
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Positive
    }

    // Class to store user memory
    public class UserMemory
    {
        public List<string> InterestTopics { get; set; } = new List<string>();
        public Dictionary<string, DateTime> TopicAccessTimes { get; set; } = new Dictionary<string, DateTime>();
        public Sentiment LastSentiment { get; set; } = Sentiment.Neutral;

        public void AddInterest(string topic)
        {
            if (!InterestTopics.Contains(topic))
            {
                InterestTopics.Add(topic);
                TopicAccessTimes[topic] = DateTime.Now;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";

            // Create an instance of the bot
            CybersecurityBot bot = new CybersecurityBot();

            // Start the bot (logo, voice greeting, ask for user name)
            bot.Start();

            // Run the main conversation loop
            bot.RunConversation();
        }
    }

    public class CybersecurityBot
    {
        public string UserName { get; private set; } // Stores the user's name
        private Dictionary<string, List<string>> topicResponses;
        private Dictionary<string, ResponseHandler> keywordHandlers;
        private Random random = new Random(); // Used to select random fallback messages
        private bool isRunning = true; // Controls the main conversation loop
        private UserMemory userMemory;
        private Dictionary<string, List<string>> sentimentResponses;

        public CybersecurityBot()
        {
            userMemory = new UserMemory();
            InitializeResponses();
            InitializeKeywordHandlers();
            InitializeSentimentResponses();
        }

        private void InitializeSentimentResponses()
        {
            sentimentResponses = new Dictionary<string, List<string>>
            {
                { "worried", new List<string> {
                    "I understand your concern. Let me help you feel more secure.",
                    "It's natural to feel worried. I'll guide you through some safety measures.",
                    "Your concerns are valid. Let's work on addressing them together."
                }},
                { "curious", new List<string> {
                    "That's a great question! I'm excited to help you learn more.",
                    "Your curiosity will help you stay safer online. Let me explain.",
                    "I'm glad you're interested in learning more about this topic!"
                }},
                { "frustrated", new List<string> {
                    "I can hear your frustration. Let's break this down into simpler steps.",
                    "Don't worry, we'll figure this out together.",
                    "It can be challenging, but I'm here to help you understand."
                }}
            };
        }

        private void InitializeKeywordHandlers()
        {
            keywordHandlers = new Dictionary<string, ResponseHandler>
            {
                { "password", HandlePasswordQuery },
                { "scam", HandleScamQuery },
                { "privacy", HandlePrivacyQuery },
                { "phishing", HandlePhishingQuery }
            };
        }

        private void InitializeResponses()
        {
            topicResponses = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "Use a combination of uppercase, lowercase, numbers, and symbols in your passwords.",
                    "Never reuse passwords across different accounts.",
                    "Consider using a password manager to generate and store strong passwords.",
                    "Make your passwords at least 12 characters long for better security."
                }},
                { "scam", new List<string> {
                    "Be wary of offers that seem too good to be true.",
                    "Never send money or personal information to unverified sources.",
                    "Check the sender's email address carefully for signs of spoofing.",
                    "When in doubt, verify requests through official channels."
                }},
                { "privacy", new List<string> {
                    "Regularly review and update your privacy settings on social media.",
                    "Be cautious about what personal information you share online.",
                    "Use encryption tools when handling sensitive data.",
                    "Consider using privacy-focused browsers and search engines."
                }},
                { "phishing", new List<string> {
                    "Always verify the sender's email address before clicking links.",
                    "Be suspicious of urgent requests for personal information.",
                    "Look for spelling and grammar errors in suspicious emails.",
                    "Hover over links to preview URLs before clicking them."
                }}
            };
        }

        private string GetRandomResponse(List<string> responses)
        {
            return responses[random.Next(responses.Count)];
        }

        private Sentiment DetectSentiment(string input)
        {
            input = input.ToLower();
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("concerned"))
                return Sentiment.Worried;
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("tell me"))
                return Sentiment.Curious;
            if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("difficult"))
                return Sentiment.Frustrated;
            if (input.Contains("thanks") || input.Contains("great") || input.Contains("helpful"))
                return Sentiment.Positive;
            return Sentiment.Neutral;
        }

        private string HandlePasswordQuery(string input, string userName, Sentiment sentiment)
        {
            userMemory.AddInterest("password");
            var response = GetRandomResponse(topicResponses["password"]);
            return AddSentimentContext(response, sentiment);
        }

        private string HandleScamQuery(string input, string userName, Sentiment sentiment)
        {
            userMemory.AddInterest("scam");
            var response = GetRandomResponse(topicResponses["scam"]);
            return AddSentimentContext(response, sentiment);
        }

        private string HandlePrivacyQuery(string input, string userName, Sentiment sentiment)
        {
            userMemory.AddInterest("privacy");
            var response = GetRandomResponse(topicResponses["privacy"]);
            return AddSentimentContext(response, sentiment);
        }

        private string HandlePhishingQuery(string input, string userName, Sentiment sentiment)
        {
            userMemory.AddInterest("phishing");
            var response = GetRandomResponse(topicResponses["phishing"]);
            return AddSentimentContext(response, sentiment);
        }

        private string AddSentimentContext(string response, Sentiment sentiment)
        {
            string sentimentPrefix = "";
            switch (sentiment)
            {
                case Sentiment.Worried:
                    sentimentPrefix = GetRandomResponse(sentimentResponses["worried"]);
                    break;
                case Sentiment.Curious:
                    sentimentPrefix = GetRandomResponse(sentimentResponses["curious"]);
                    break;
                case Sentiment.Frustrated:
                    sentimentPrefix = GetRandomResponse(sentimentResponses["frustrated"]);
                    break;
            }
            return string.IsNullOrEmpty(sentimentPrefix) ? response : $"{sentimentPrefix}\n{response}";
        }

        public void Start()
        {
            DisplayLogo(); // Show the ASCII banner
            PlayVoiceGreeting(); // Speak the welcome message using speech synthesis
            GetUserName(); // Ask the user for their name
            DisplayWelcomeMessage(); // Show a welcome message using the name
        }

        private void DisplayLogo()
        {
            // Code Attribution
            // This method was taken from stakeoverflow
            // https://stackoverflow.com/questions/8946847/how-do-i-change-the-console-text-color-in-c
            // hellow
            // https://stackoverflow.com/users/1021920/hellow

            // Displays an ASCII art logo with colors
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
  ______   ______  _____ ____  ____  _____ ____ _   _ ____  ___ _______   __ 
 / ___\ \ / / __ )| ____|  _ \/ ___|| ____/ ___| | | |  _ \|_ _|_   _\ \ / / 
| |    \ V /|  _ \|  _| | |_) \___ \|  _|| |   | | | | |_) || |  | |  \ V /  
| |___  | | | |_) | |___|  _ < ___) | |__| |___| |_| |  _ < | |  | |   | |   
 \____| |_| |____/|_____|_| \_\____/|_____\____|\___/|_| \_\___| |_|_ _|_|_  
   / \ \      / / \  |  _ \| ____| \ | | ____/ ___/ ___|  | __ ) / _ \_   _| 
  / _ \ \ /\ / / _ \ | |_) |  _| |  \| |  _| \___ \___ \  |  _ \| | | || |   
 / ___ \ V  V / ___ \|  _ <| |___| |\  | |___ ___) |__) | | |_) | |_| || |   
/_/   \_\_/\_/_/   \_\_| \_\_____|_| \_|_____|____/____/  |____/ \___/ |_|        
");
            Console.ResetColor();
        }

        private void PlayVoiceGreeting()
        {

            // Display a welcome message visually
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║      Welcome to the Cybersecurity        ║");
            Console.WriteLine("║           Awareness Bot!                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                // Code Attribution
                // This method was taken from stakeoverflow
                // https://stackoverflow.com/questions/1985403/how-to-use-the-speechsynthesizer-in-c
                // Jeffrey Kemp
                // https://stackoverflow.com/users/103295/jeffrey-kemp

                // Use speech synthesizer to say the welcome message
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    synth.SetOutputToDefaultAudioDevice();
                    synth.Speak("Welcome to the Cybersecurity Awareness Bot. Let's learn how to stay safe online!");
                }
            }
            catch (Exception)
            {   // Code Attribution
                // This method was taken from stakeoverflow
                // https://stackoverflow.com/questions/21208808/how-to-make-the-console-text-appear-as-if-its-being-typed-in-real-time 
                // Gordon Linoff
                // https://stackoverflow.com/users/1144035/gordon-linoff

                // If speaking fails, show an error and continue
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Note: Voice greeting could not be played. Continuing without audio.");
                Console.ResetColor();
                Thread.Sleep(1000);
            }
        }

        private void GetUserName()
        {
            // Ask the user for their name and store it
            Console.ForegroundColor = ConsoleColor.Green;
            TypeText("What's your name? ");
            Console.ResetColor();

            string input = Console.ReadLine().Trim();

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TypeText("Please enter a valid name: ");
                Console.ResetColor();
                input = Console.ReadLine().Trim();
            }

            UserName = input;
        }

        private void DisplayWelcomeMessage()
        {
            // Clear screen and show welcome message using the user's name
            Console.Clear();
            DisplayLogo();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine($"║  Welcome, {UserName.PadRight(29)} ║");
            Console.WriteLine("║  I'm here to help you with cybersecurity ║");
            Console.WriteLine("║  questions and concerns.                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            TypeText("\nYou can ask me about password safety, phishing, safe browsing, and more!\n");
            TypeText("Type 'exit' at any time to quit.\n\n");
            Console.ResetColor();
        }
        // Code attribution 
        // This method was taken from stackoverflow
        // https://stackoverflow.com/questions/78668431/how-can-i-get-my-ai-chatbot-to-remember-past-answers
        // ShadowFly
        // https://stackoverflow.com/users/22779755/shadowfly
        public void RunConversation()
        {
            // Main loop to keep the conversation going
            while (isRunning)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{UserName}> ");
                Console.ResetColor();

                string input = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TypeText("I didn't quite understand that. Could you rephrase?\n");
                    Console.ResetColor();
                    continue;
                }

                ProcessInput(input);
            }
        }
        // Code attribution 
        // This method was taken from stackoverflow
        // https://stackoverflow.com/questions/60392335/how-to-provide-selection-options-as-response-in-chatterbot
        // Mansoor Ramzani
        // https://stackoverflow.com/users/12959268/mansoor-ramzani
        private void ProcessInput(string input)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("CyberBot> ");
            Console.ResetColor();

            // Check for exit commands
            if (input.ToLower() == "exit" || input.ToLower() == "quit" || input.ToLower() == "bye")
            {
                TypeText($"Goodbye, {UserName}! Stay secure out there!\n\n");
                isRunning = false;
                return;
            }

            // Detect sentiment
            var sentiment = DetectSentiment(input);
            userMemory.LastSentiment = sentiment;

            // Check for memory recall
            if (input.ToLower().Contains("what did we discuss") || input.ToLower().Contains("what topics"))
            {
                if (userMemory.InterestTopics.Any())
                {
                    TypeText($"We've discussed {string.Join(", ", userMemory.InterestTopics)}. Would you like to know more about any of these topics?\n\n");
                    return;
                }
            }
            // Code attribution 
            // This method was taken from stackoverflow
            // https://stackoverflow.com/questions/58979356/create-a-valid-conversationreference-from-scratch-to-send-proactive-messages
            // mdrichardson
            // https://stackoverflow.com/users/12248433/mdrichardson


            // Process through keyword handlers
            foreach (var handler in keywordHandlers)
            {
                if (input.ToLower().Contains(handler.Key))
                {
                    string response = handler.Value(input, UserName, sentiment);
                    TypeText(response + "\n\n");
                    return;
                }
            }

            // Default response if no keywords matched
            TypeText("I'm not sure about that. You can ask me about passwords, scams, privacy, or phishing. Or ask what topics we've discussed!\n\n");
        }
        // Code attribution 
        // This method was taken from stackoverflow
        // https://stackoverflow.com/questions/54593082/simple-sentiment-analysis-using-ml-net-and-ienumerable-dataview
        // Vikash Rathee
        // https://stackoverflow.com/users/1610100/vikash-rathee
        private void TypeText(string text, int delay = 20)
        {
            // Simulates typing effect
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
        }
    }
}