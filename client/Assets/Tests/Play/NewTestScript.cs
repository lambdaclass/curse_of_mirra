using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ColorInputTest
{
    // Test black color
    [Test]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.5f, 0.5f, 0.5f, 0)]
    [TestCase(0, 0, 0, 1)]
    public void ColorInputTestBlack(float r, float g, float b, float a)
    {
        r = Mathf.Round(r);
        g = Mathf.Round(g);
        b = Mathf.Round(b);
        a = Mathf.Round(a);

        // assign color based on input color
        Color newColor = new Color(r, g, b, a);

        // Use the Assert class to test conditions
        Assert.That(newColor, Is.EqualTo(Color.black));
    }

    // Test White color
    [Test]
    [TestCase(1, 1, 1, 1)]
    [TestCase(0.5f, 0.5f, 0.5f, 0)]
    [TestCase(0, 0, 0, 1)]
    public void ColorInputTestWhite(float r, float g, float b, float a)
    {
        r = Mathf.Round(r);
        g = Mathf.Round(g);
        b = Mathf.Round(b);
        a = Mathf.Round(a);

        // assign color based on input color
        Color newColor = new Color(r, g, b, a);

        // Use the Assert class to test conditions
        Assert.That(newColor, Is.EqualTo(Color.white));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
