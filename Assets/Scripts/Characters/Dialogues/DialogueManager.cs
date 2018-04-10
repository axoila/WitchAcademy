using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Yarn;

public class DialogueManager : MonoBehaviour {

	public static DialogueManager instance;

	public Text nameField;
	public Text contentField;
	[Range(0f, 0.1f)]
	public float characterDelay;

	public string abortCharacters;

	public DialogueButton[] optionButtons;

	//Yarn stuff
	Dialogue dialogue;
	bool dialogueRunning = false;
	bool showingOptions = false;

	[HideInInspector]
	public Character talker;
	[HideInInspector]
	public Character listener;

	Dictionary<string, ProceduralString> customStrings;

	// Use this for initialization
	void Awake () {
		if(instance == null){
            instance = this;
        } else if(instance != this) {
            instance = this;
            Debug.LogWarning("new DialogueManager set");
        }

		for(int i=0;i<optionButtons.Length;i++){
			optionButtons[i].button.enabled = false;
			optionButtons[i].buttonBG.enabled = false;
			optionButtons[i].textField.enabled = false;
		}

		gameObject.SetActive(false);

		customStrings = new Dictionary<string, ProceduralString>();
		customStrings.Add("name",
			() => {
				return listener.name;
			}
		);
	}

	public Coroutine StartDialogue(Character talker, Character listener, Conversation convo){
		this.talker = talker;
		this.listener = listener;

		TimeManager.worldTime.paused = true;
		dialogueRunning = true;
		gameObject.SetActive(true);
		nameField.text = talker.name;

		return StartCoroutine(RunDialogue(convo));
	}

	public void StopDialogue(){
		StopAllCoroutines();
		TimeManager.worldTime.paused = false;
		dialogueRunning = false;
		gameObject.SetActive(false);
	}

	public IEnumerator RunDialogue(Conversation convo){
		dialogue = new Dialogue(talker.blackboards.GetBoard(listener));
		dialogue.LogDebugMessage = delegate (string message) {
			Debug.Log (message);
		};
		dialogue.LogErrorMessage = delegate (string message) {
			Debug.LogError (message);
		};
		dialogue.LoadString(convo.conversation.text, convo.conversation.name);

		bool firstLine = true;
		foreach (Yarn.Dialogue.RunnerResult step in dialogue.Run(convo.startNode)) {
			if (step is Yarn.Dialogue.LineResult) {
				if(!firstLine){
					TimeManager.worldTime.StartCoroutine(TimeManager.worldTime.FastForward(1));
				} else{
					firstLine = false;
				}
				// Wait for line to finish displaying
				var lineResult = step as Yarn.Dialogue.LineResult;
				yield return StartCoroutine (ShowLine(lineResult.line.text));

				// Wait for any user input
				while (Input.anyKeyDown == false) {
					yield return null;
				}

			} else if (step is Yarn.Dialogue.OptionSetResult) {
				TimeManager.worldTime.StartCoroutine(TimeManager.worldTime.FastForward(1));
				// Wait for user to finish picking an option
				var optionSetResult = step as Yarn.Dialogue.OptionSetResult;
				yield return StartCoroutine (
					ShowOptions (
						optionSetResult.options,
						optionSetResult.setSelectedOptionDelegate
					)
				);

			} else if (step is Yarn.Dialogue.CommandResult) {

				// Wait for command to finish running

				var commandResult = step as Yarn.Dialogue.CommandResult;

				if (DispatchCommand(commandResult.command.text)) {
					// command was dispatched
				} else {
					Debug.LogWarning(commandResult.command.text + " could not be executed.");
				}

			} else if(step is Yarn.Dialogue.NodeCompleteResult) {

				// Wait for post-node action
				//var nodeResult = step as Yarn.Dialogue.NodeCompleteResult;
				//yield return StartCoroutine (this.dialogueUI.NodeComplete (nodeResult.nextNode));
			}
		}

		TimeManager.worldTime.paused = false;
		dialogueRunning = false;
		gameObject.SetActive(false);
	}

	IEnumerator ShowLine(string content, string name = null){
		print("showing line " + content + "...");

		contentField.enabled = true;
		for(int i=0;i<optionButtons.Length;i++){
			optionButtons[i].button.enabled = false;
			optionButtons[i].buttonBG.enabled = false;
			optionButtons[i].textField.enabled = false;
		}

		if(!string.IsNullOrEmpty(name))
			nameField.text = name;
		string text = "";
		if(characterDelay > 0){
			for(int i=0; i<content.Length; i++){
				text += content[i];
				contentField.text = text;
				yield return new WaitForSecondsRealtime(characterDelay);
			}
		}
		contentField.text = content;
	}

	IEnumerator ShowOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser){
		contentField.enabled = false;
		if (optionsCollection.options.Count > optionButtons.Length) {
			Debug.LogWarning("There are more options to present than there are" +
							"buttons to present them in. This will cause problems.");
		}

		showingOptions = true;

		for(int i=0; i<Math.Min(optionButtons.Length, optionsCollection.options.Count); i++){
			optionButtons[i].button.enabled = true;
			optionButtons[i].buttonBG.enabled = true;
			optionButtons[i].textField.enabled = true;
			optionButtons[i].textField.text = optionsCollection.options[i];
			int index = i;
			optionButtons[i].button.onClick.AddListener(() => 
					{
						showingOptions = false;
						optionChooser(index);
					}
			);
		}

		while(showingOptions)
			yield return null;
		
		for(int i=0;i<optionButtons.Length;i++){
			optionButtons[i].button.enabled = false;
			optionButtons[i].buttonBG.enabled = false;
			optionButtons[i].textField.enabled = false;
			optionButtons[i].button.onClick.RemoveAllListeners();
		}
	}

	/// commands that can be automatically dispatched look like this:
	/// COMMANDNAME OBJECTNAME <param> <param> <param> ...
	/** We can dispatch this command if:
		* 1. it has at least 2 words
		* 2. the second word is the name of an object
		* 3. that object has components that have methods with the YarnCommand attribute that have the correct commandString set
		*/
	public bool DispatchCommand(string command) {

		var words = command.Split(' ');

		// need 2 parameters in order to have both a command name
		// and the name of an object to find
		if (words.Length < 2)
			return false;

		var commandName = words[0];

		var objectName = words[1];

		var sceneObject = GameObject.Find(objectName);

		// If we can't find an object, we can't dispatch a command
		if (sceneObject == null)
			return false;

		int numberOfMethodsFound = 0;
		List<string[]> errorValues = new List<string[]>();

		List<string> parameters;

		if (words.Length > 2) {
			parameters = new List<string>(words);
			parameters.RemoveRange(0, 2);
		} else {
			parameters = new List<string>();
		}

		// Find every MonoBehaviour (or subclass) on the object
		foreach (var component in sceneObject.GetComponents<MonoBehaviour>()) {
			var type = component.GetType();

			// Find every method in this component
			foreach (var method in type.GetMethods()) {

				// Find the YarnCommand attributes on this method
				var attributes = (YarnCommandAttribute[]) method.GetCustomAttributes(typeof(YarnCommandAttribute), true);

				// Find the YarnCommand whose commandString is equal to the command name
				foreach (var attribute in attributes) {
					if (attribute.commandString == commandName) {


						var methodParameters = method.GetParameters();
						bool paramsMatch = false;
						// Check if this is a params array
						if (methodParameters.Length == 1 && methodParameters[0].ParameterType.IsAssignableFrom(typeof(string[])))
							{
								// Cool, we can send the command!
								string[][] paramWrapper = new string[1][];
								paramWrapper[0] = parameters.ToArray();
								method.Invoke(component, paramWrapper);
								numberOfMethodsFound++;
								paramsMatch = true;

						}
						// Otherwise, verify that this method has the right number of parameters
						else if (methodParameters.Length == parameters.Count)
						{
							paramsMatch = true;
							foreach (var paramInfo in methodParameters)
							{
								if (!paramInfo.ParameterType.IsAssignableFrom(typeof(string)))
								{
									Debug.LogErrorFormat(sceneObject, "Method \"{0}\" wants to respond to Yarn command \"{1}\", but not all of its parameters are strings!", method.Name, commandName);
									paramsMatch = false;
									break;
								}
							}
							if (paramsMatch)
							{
								// Cool, we can send the command!
								method.Invoke(component, parameters.ToArray());
								numberOfMethodsFound++;
							}
						}
						//parameters are invalid, but name matches.
						if (!paramsMatch)
						{
							//save this error in case a matching command is never found.
							errorValues.Add(new string[] { method.Name, commandName, methodParameters.Length.ToString(), parameters.Count.ToString() });
						}
					}
				}
			}
		}

		// Warn if we found multiple things that could respond
		// to this command.
		if (numberOfMethodsFound > 1) {
			Debug.LogWarningFormat(sceneObject, "The command \"{0}\" found {1} targets. " +
				"You should only have one - check your scripts.", command, numberOfMethodsFound);
		} else if (numberOfMethodsFound == 0) {
			//list all of the near-miss methods only if a proper match is not found, but correctly-named methods are.
			foreach (string[] errorVal in errorValues) {
				Debug.LogErrorFormat(sceneObject, "Method \"{0}\" wants to respond to Yarn command \"{1}\", but it has a different number of parameters ({2}) to those provided ({3}), or is not a string array!", errorVal[0], errorVal[1], errorVal[2], errorVal[3]);
			}
		}

		return numberOfMethodsFound > 0;
	}

	[Serializable]
	public struct DialogueButton{
		public Button button;
		public Image buttonBG;
		public Text textField;
	}
}

/// Apply this attribute to methods in your scripts to expose
/// them to Yarn.

/** For example:
*  [YarnCommand("dosomething")]
*      void Foo() {
*         do something!
*      }
*/
public class YarnCommandAttribute : System.Attribute
{
	public string commandString { get; private set; }

	public YarnCommandAttribute(string commandString) {
		this.commandString = commandString;
	}
}

public delegate string ProceduralString();