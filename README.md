# Options

Options is a .NET library that replicates the option type from functional programming with an API that is palatable to less-functional CLR languages.

## Using Options In Null's Stead

Null references are often used to indicate that no sensible value was found or could be created. For example, the following code returns null if no sliced bread is found.

    public Bread GetSliced()
    {
        return _breads.SingleOrDefault(b => b.Type == BreadType.Sliced);
    }
    
 You need to call a method on that returned value. Even worse, you might need to tuck it away for future use. Pretty soon, your code is littered with `if (slicedBread != null) {...}` if you're lucky. If you're unlucky someone forgets to check that reference and things get all exception-y.
 
 If you're the consumer of a method like this, Options can help.
 
     var maybeSlicedBread = Option.Create(_breadManager.GetSliced());
     
 Now you can't get at that value without acknowledging it might not be there.
 
 If you're the creator of a method like this, Options can help you, too.
 
    public Option<Bread> GetSliced()
    {
        return _breads.OptionSingle(b => b.Type == BreadType.Sliced);
    }
    
There are plenty of friendly methods to help you out. Check them out. I hope you like them.