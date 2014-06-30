﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Fitter.Core {
  public class Builder {
    public dynamic Build(object spec, int maxDepth = 5) {
      var entries = _EntryBuilder.Build(spec ?? _Empty);
      UpdateEntries(maxDepth, entries);
      return BuildResult(entries);
    }

    private static dynamic BuildResult(SpecEntry[] entries) {
      IDictionary<string, object> result = new ExpandoObject();
      Array.ForEach(entries, se => result[se.Name] = se.Value);
      return result;
    }

    private static void UpdateEntries(int maxDepth, SpecEntry[] entries) {
      UpdateEntries(entries);
      if ((--maxDepth > 0) && (entries.Any(e => e.NeedsUpdate)))
        UpdateEntries(maxDepth, entries);
    }

    private static void UpdateEntries(SpecEntry[] entries) {
      foreach (var entry in entries)
        Array.ForEach(entries, se => se.ApplyTo(entry));
    }

    private static readonly object _Empty = new {};
    private static readonly SpecEntryBuilder _EntryBuilder = new SpecEntryBuilder();
  }
}