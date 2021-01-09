# CurlyBracketsAnalyzer

This analyzer generates a syntax warning for any statement that is not enclosed in a block that has curly brackets { and }.
If that is the case, the analyzer will suggest a code fix which is basically will add missing curly brackets { and }.

## Before

```csharp
if (obj == null)
  return false;
```

## After

```csharp
if (obj == null)
{
  return false;
}
```

---

<div style="display: flex; justify-content: space-between">
  <a href="../README.md"> ⬅ Back To Home </a>
  <a href="../NullEqualityAnalyzer/NullEqualityAnalyzer.md"> ➡ Next </a>
</div>
