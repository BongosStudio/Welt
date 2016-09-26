// you can import namespaces by using 'import namespace'
import WsCore

// you can either explicitly state the namespace or allow the compiler to use the directory it's in
namespace Examples 
{
	// for entrance points in the script, the class name can be anything. To call the script, you'll simply use 'wsc helloworld'
	// and the compiler will look for the entrace function.
	class HelloWorld
	{
		// creates an instance string "Name". Notice semicolons aren't needed.
		public Name: string
		
		// declares function Main. It accepts a single argument of a string array and 
		// returns an int object.
		static func Main(args: []string) : int
		{
			// Prints the input to the console. You can also use print or println
			printf("Hello world!")

			return 0
		}
	}
}