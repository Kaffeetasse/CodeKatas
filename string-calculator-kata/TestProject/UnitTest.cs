using string_calculator_kata;

namespace TestProject;

public class UnitTest
{
    [Fact]
    public void AddEmpty_ReturnZero()
    {
        var result = StringCalculator.Add("");

        Assert.Equal(0, result);
    }
    
    [Theory]
    [InlineData("1", 1)]
    [InlineData("1,2", 3)]
    [InlineData("1\n2,3", 6)]
    [InlineData("1,\n", 0)]
    public void AddOneOrTwoNumbers_ReturnSum(string numbers, int expected)
    {
        var result = StringCalculator.Add(numbers);

        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void AddRandomNumbers_ReturnSum()
    {
        var random = new Random();
        
        // generate a random number between 10 and 50
        // and execute the test that many times
        var countOfExecutions = random.Next(10, 50);
        
        for (var i = 0; i < countOfExecutions; i++)
        {
            var numbers = (Enumerable.Range(0, random.Next(1, 100)).Select(_ => random.Next(1, 100).ToString())).ToList();
            var expected = numbers.Sum(int.Parse);

            var result = StringCalculator.Add(string.Join(",", numbers));

            Assert.Equal(expected, result);
        }
    }
    
    [Theory]
    [InlineData("//;\n1;2", 3)]
    [InlineData("//#\n1#2#3", 6)]
    [InlineData("//$\n1$2$3$4", 10)]
    [InlineData("//-\n1-2-3-4-5", 15)]
    [InlineData("//$$\n1$$2$$3$$4", 10)]
    [InlineData("//$$$\n1$$$2$$$3$$$4", 10)]
    [InlineData("//***\n1***2***3", 6)]
    public void AddWithDelimiter_ReturnSum(string numbers, int expected)
    {
        var result = StringCalculator.Add(numbers);

        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("-1", "negatives not allowed: -1")]
    [InlineData("-1,-2", "negatives not allowed: -1,-2")]
    [InlineData("1,-2", "negatives not allowed: -2")]
    [InlineData("//$\n-1$2$3$-4", "negatives not allowed: -1,-4")]
    public void AddNegativeNumbers_ThrowException(string numbers, string expected)
    {
        var exception = Assert.Throws<Exception>(() => StringCalculator.Add(numbers));

        Assert.Equal(expected, exception.Message);
    }
    
    [Theory]
    [InlineData("2,1001", 2)]
    [InlineData("2,1000", 1002)]
    [InlineData("2,1000,1001", 1002)]
    [InlineData("2,1000,1001,1002", 1002)]
    [InlineData("//;\n2;1001", 2)]
    public void AddNumbersLargerThan1000_ReturnSum(string numbers, int expected)
    {
        var result = StringCalculator.Add(numbers);

        Assert.Equal(expected, result);
    }
    
    // check for alternative delimiters
    [Theory]
    [InlineData("//[**][&]\n1**2&3", 6)]
    [InlineData("//[**][&][%]\n1**2&3%4", 10)]
    [InlineData("//[**][&][%][#]\n1**2&3%4#5", 15)]
    [InlineData("//[**][&b][%p][#r]\n1**2&b3%p4#r5", 15)]
    public void AddWithMultipleDelimiters_ReturnSum(string numbers, int expected)
    {
        var result = StringCalculator.Add(numbers);

        Assert.Equal(expected, result);
    }
}