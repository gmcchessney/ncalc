# Description

NCalc is a set of assemblies allowing expressions evaluation. The main class to use is **Expression**.
This class has a method **Evaluate()** returning the actual value of its **String** representation.

Example:

```c#
  Expression e = new Expression("2 * 3");
  object result = e.Evaluate();
  
  Console.WriteLine(result);
```
This example above first creates an instance of **Expression** using a valued constructor. This constructor takes a string as parameter.
Then the method **Evaluate()** is called to parse the string, and returns the actual value represented by the string.

To create expressions you can combine several [[operators]] and [[values]].

## Case sensitivity

By default, the evaluation process is case sensitive. This means every parameter and function evaluation will match using case. This behavior can be overriden using a specific evaluation option.

```c#
 Expression e = new Expression("aBs(-1)", ExpressionOptions.IgnoreCase)
 Debug.Assert(1M, e.Evaluate());
```

## Handling errors

When the expression has a syntax error, the evaluation will throw an **EvaluationException**.

```c#
 try
 {
     new Expression("(3 + 2").Evaluate();
 }
 catch(EvaluationException e)
 {
     Console.WriteLine("Error catched: " + e.Message);
 }
```

Though, you can also detect syntax errors before the evaluation by using the **HasErrors()** method.

```c#
 Expression e = new Expression("a + b * (");
 if(e.HasErrors())
 {
     Console.WriteLine(e.Error);
 }
```

## Expression caching

When **Evaluate()** is called on an expression, it is compiled once. If the same expression is reused the compilation is not executed again. Thus you can reuse **Expression** instances by changing the parameters, and you will gain in performance because only the trasversal of the expression tree will be done.

Moreover, each compiled expression is cached internaly, which means you don't even have to care about reusing an **Expression** instance, the framework will do it for you. The cache is automatically cleaned like the GC does when an Expression is no more used, or memory is needed (i.e. using **WeakReference**). 

You can disable this behavior at the framework level by setting false to **CacheEnabled**.

```c#
 Expression.CacheEnabled = false;
```

You can also tell a specific **Expression** instance not to be taken from the cache.

```c#
 Expression e = new Expression("1 + 1", ExpressionOptions.NoCache);
```