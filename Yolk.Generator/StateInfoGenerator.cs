namespace Yolk.Generator {
  using System.Collections.Immutable;
  using System.Text;
  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp.Syntax;
  using Microsoft.CodeAnalysis.Text;

  /// <summary>
  /// Generates a partial class that implements IStateInfo for classes decorated with the StateInfo attribute.
  /// </summary>
  [Generator]
  public class StateInfoGenerator : IIncrementalGenerator {

    /// <summary>
    /// Generates a partial class that implements IStateInfo for classes decorated with the StateInfo attribute.
    /// </summary>
    /// <param name="context"></param>
    public void Initialize(IncrementalGeneratorInitializationContext context) {
      var classDeclarations = context.SyntaxProvider
      .CreateSyntaxProvider(
          predicate: (s, _) => IsSyntaxTargetForGeneration(s),
          transform: (ctx, _) => GetSemanticTargetForGeneration(ctx))
      .Where(c => c != null)
      .Select((c, _) => c)
      .Collect();

      var compilationAndClasses = context.CompilationProvider
          .Combine(classDeclarations);

      context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
          Execute(source.Left, source.Right, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0;

    private static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
      var classSyntax = (ClassDeclarationSyntax)context.Node;
      foreach (var attributeList in classSyntax.AttributeLists) {
        foreach (var attribute in attributeList.Attributes) {
          var symbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;
          if (symbol is IMethodSymbol methodSymbol &&
              methodSymbol.ContainingType.Name == "StateInfoAttribute") {
            return classSyntax;
          }
        }
      }
      return null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context) {
      var interfaceSource = $@"
namespace Yolk {{
  public interface IStateInfo {{
    string Name {{  get; }}
    string State {{ get; }}
  }}
}}";

      context.AddSource($"StateInfo.g.cs", SourceText.From(interfaceSource, Encoding.UTF8));

      foreach (var classDecl in classes) {
        var className = classDecl.Identifier.Text;
        var ns = classDecl.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
          ?? classDecl.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString()
          ?? "Generated";

        var source = $@"
namespace {ns}
{{
    using Yolk;

    public partial class {className} : IStateInfo
    {{
        string IStateInfo.Name => Name;
        string IStateInfo.State => Logic.Value.ToString();

        public override void _Ready() => AddToGroup(""state"");
    }}
}}";
        context.AddSource($"{className}_StateInfo.g.cs", SourceText.From(source, Encoding.UTF8));
      }
    }
  }
}
