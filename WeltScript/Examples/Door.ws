// WARNING: this is at early stages of development. API is subject to change.
import WsCore.IO

class Door
{
	static func Main(args: []string) : int
	{
		// while statements will cycle through the statement until the condition is false.
		// to have it going indefinitely until break is called, use 'while (true)'
		while (Bus.IsConnected)
		{
			// use $ to declare a new local variable within the statement
			$level = Bus.Get(PIN_12) // gets the power level on the bus at pin 12. Max is PIN_24

			if ($level > 0) // checks if the power level is higher than 0. 0 means not power and 15 means full power.
			{
				Bus.Set(PIN_8, 15) // sets the power level on the bus at pin 8 to max. 
			}
			else
			{
				sleep(100) // causes the while statement to wait for 100 milliseconds
			}
		}
	}
}