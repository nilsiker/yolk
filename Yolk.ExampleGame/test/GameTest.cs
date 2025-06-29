namespace Yolk.Game.Test;

using System.Threading.Tasks;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Godot;

public class GameTest(Node testScene) : TestClass(testScene) {
  private Game _game = default!;
  private Fixture _fixture = default!;

  [SetupAll]
  public async Task Setup() {
    _fixture = new Fixture(TestScene.GetTree());
    _game = await _fixture.LoadAndAddScene<Game>();
  }

  [CleanupAll]
  public void Cleanup() => _fixture.Cleanup();

  [Test]
  public void TestButtonUpdatesCounter() {
  }
}
