# NullEqualityAnalyzer

This analyzer generates a syntax warning for any statement using equality operator to compare an operand with null.
If that is the case, the analyzer will suggest a code fix which is basically will replace comparison with [pattern matching](https://devblogs.microsoft.com/premier-developer/dissecting-the-pattern-matching-in-c-7/).

## Before

```csharp
if (obj == null)
{
  return false;
}
```

## After

```csharp
if (obj is null)
{
  return false;
}
```

---

<div style="display: flex; justify-content: space-between">
  <a href="../README.md"> ⬅ Back To Home </a>
  <a href="../CurlyBracketsAnalyzer/CurlyBracketsAnalyzer.md"> ⬅ Previous </a>
</div>
