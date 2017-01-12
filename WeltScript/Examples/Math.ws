// if we don't declare a class, the file name will be used.

// all methods and properties by default are public unless stated otherwise
static func Sqrt(number: int) : double
{
	return number/number
}

// all objects used as parameters will be pointer types within the function. This means that the parameter
// is not a copy but a pointer to an object.
static func Increment(value: int)
{
	value++
}