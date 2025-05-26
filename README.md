[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/4cldIbLm)
# Cybersecurity Awareness Bot

An intelligent command-line bot designed to provide dynamic, context-aware information about cybersecurity best practices. The bot features sentiment analysis, memory capabilities, and personalized responses to help users better understand and implement cybersecurity measures.

## Enhanced Features

### Dynamic Responses
* **Random Response Selection:** Multiple informative responses for each cybersecurity topic
* **Context-Aware Answers:** Responses tailored to the conversation context
* **Sentiment Detection:** Bot adapts its tone based on user's emotional state (worried, curious, frustrated)

### Memory and Learning
* **Topic Tracking:** Remembers discussed topics for future reference
* **Interest Memory:** Stores user's cybersecurity interests
* **Conversation History:** Ability to recall and reference previous discussions

### Core Cybersecurity Topics
* Password Safety
* Scam Prevention
* Privacy Protection
* Phishing Awareness

### Interactive Features
* **Personalized Greeting:** Custom welcome message with user's name
* **Voice Synthesis:** Text-to-speech welcome message (system dependent)
* **Typing Animation:** Engaging text display effect
* **Color-Coded Interface:** Enhanced readability with colored text
* **ASCII Art Logo:** Professional presentation

### Smart Interaction
* **Sentiment Analysis:** Detects user emotions:
  * Worried/Concerned
  * Curious/Interested
  * Frustrated/Confused
  * Positive/Satisfied
* **Empathetic Responses:** Adjusts tone based on detected sentiment
* **Topic Memory:** "What did we discuss?" feature for topic recall

## Technical Implementation

### Key Components
* **Generic Collections:**
  * `Dictionary<string, List<string>>` for multiple responses per topic
  * `Dictionary<string, ResponseHandler>` for keyword handling
  * `List<string>` for tracking user interests
  * `Dictionary<string, DateTime>` for topic timing

* **Delegates:**
  * `ResponseHandler` delegate for sentiment-aware responses
  * Keyword-specific handlers for specialized responses

* **Classes:**
  * `UserMemory` for storing conversation history and preferences
  * `CybersecurityBot` main bot implementation
  * Custom sentiment detection and response generation

## How to Run

1. **Prerequisites:**
   * .NET SDK (latest version recommended)
   * Windows OS for voice synthesis support

2. **Build the Project:**
   ```bash
   dotnet build
   ```

3. **Run the Application:**
   ```bash
   dotnet run
   ```

## Usage Guide

1. **Starting a Conversation:**
   * Bot will display welcome message and request your name
   * Voice greeting will play if system supports it

2. **Asking Questions:**
   Example queries:
   * "Tell me about password safety"
   * "I'm worried about online scams"
   * "I'm curious about privacy"
   * "What topics have we discussed?"

3. **Understanding Responses:**
   * Bot will detect your sentiment and adjust its tone
   * Responses include both emotional support and technical information
   * Multiple variations of responses keep conversation engaging

4. **Using Memory Features:**
   * Ask "what did we discuss?" to review previous topics
   * Bot remembers your interests for personalized responses
   * Previous topics can be referenced for deeper discussions

5. **Ending the Session:**
   * Type "exit", "quit", or "bye" to end conversation
   * Bot will provide a farewell message with security reminder

## Code Structure

### Main Classes
* **Program:** Application entry point and bot initialization
* **CybersecurityBot:** Core bot logic and conversation handling
* **UserMemory:** Conversation history and user preference tracking
* **Sentiment:** Enum for emotional state tracking

### Key Methods
* `DetectSentiment()`: Analyzes user input for emotional content
* `HandleTopicQuery()`: Processes topic-specific responses
* `AddSentimentContext()`: Adjusts responses based on user sentiment
* `ProcessInput()`: Main input processing and response generation

## Future Enhancements

* **Advanced NLP:** Implement more sophisticated natural language processing
* **Machine Learning:** Add learning capabilities for response improvement
* **Multi-language Support:** Extend to multiple languages
* **User Profiles:** Save and load user preferences
* **Interactive Tutorials:** Add guided cybersecurity lessons
* **External Resources:** Integration with cybersecurity news and updates
* **Security Assessment:** Add basic security posture evaluation
* **Customizable Topics:** Allow adding new cybersecurity topics

## Contributing

Contributions are welcome! Please feel free to submit pull requests with new features, bug fixes, or documentation improvements.

## License

This project is open source and available under the MIT License.

## Acknowledgments

* Built with .NET Core
* Uses System.Speech for voice synthesis
* Inspired by modern chatbot design principles
* Implements cybersecurity best practices