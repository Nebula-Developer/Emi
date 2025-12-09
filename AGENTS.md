# Emi Agent Instructions


### Execution Policy
- Never run the `Emi.Sandbox` project on your own; building it is allowed for compilation checks or static analysis. Leave the execution up to the user, for providing a response.

### Documentation Rules (XML doc comments)
- Always add documentation tags for public/protected members (things that are accessible outside of the framework).
- Summary sentences end with a period; everything else is a single-line phrase, no period.
- Required tags to include where relevant:
  - `<summary>` — main description, brief, one/two sentences (not strict), end sentences with a period.
  - `<param name="...">` — brief description, *do not end with a full stop*.
  - `<typeparam name="...">` — brief, single-phrase description, *do not end with a full stop*.
  - `<value>` — (for properties) brief phrase, *do not end with a full stop*.
  - `<returns>` — short phrase describing the return value, *do not end with a full stop*, *only for methods, not properties*.
- Optionally utilize:
  - `<remarks>` — for additional information that doesn't fit in summary.
  - `<example>` — for short usage examples. Only when it adds clarity.
- Describe properties in terms of their value, not action. (e.g., "The current count of items." not "Gets the current count of items.")
- Always use `<see cref="..."/>` to reference types - never reference by literal name.
- Don’t include credentials, secrets, or personal information in any doc or code comment.
- If uncertain about intended behavior, leave a short `// TODO: clarify behavior` or just ask. Never assume/guess.
- When you change code, make sure the affected xml doc comments are updated accordingly if relevant.

#### Example:
  ```csharp
  /// <summary>
  /// The maximum retry count.
  /// </summary>
  public int MaxRetries { get; }

  /// <summary>
  /// Clears the cache and refreshes entries.
  /// </summary>
  /// <param name="force">Clear all entries immediately</param>
  /// <returns>Whether the cache refresh was successful</returns>
  public bool RefreshCache(bool force)
  {
      // ...
  }

  /// <summary>
  /// Creates a new instance of <see cref="Box{T}"/> with the provided value.
  /// </summary>
  /// <typeparam name="T">Type stored in the box</typeparam>
  /// <param name="value">Initial value</param>
  /// <returns>New boxed value</returns>
  public static Box<T> Create<T>(T value)
  {
      // ...
  }
  ```

### Testing
- This project uses XUnit for testing, in the `Emi.Tests` project.
- When adding/updating functionality, always include/update relevant unit tests.
- Unit tests shouldn't just test "happy paths" but also edge cases and error conditions.
- Make sure tests are isolated, and feel free to use unified setup/teardown methods for common initialization/cleanup. Do not duplicate code across tests.
- Make sure tests work after your changes, and that they pass consistently.
- The file structure in `Emi.Tests` should mirror that of `Emi` for easy navigation. Eg, any tests for `Emi/Core/Element_Composite.cs` should go in `Emi.Tests/Core/Element_Composite_Tests.cs`.
- Test methods should be named clearly to indicate what they are testing, e.g., `MethodName_StateUnderTest_ExpectedBehavior`.