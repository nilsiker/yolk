namespace Yolk.Game.Test;

using System;
using System.Threading.Tasks;
using Chickensoft.AutoInject;
using Chickensoft.GoDotTest;
using Chickensoft.GodotTestDriver;
using Godot;
using Moq;
using Shouldly;

public class GameTest(Node testScene) : TestClass(testScene), System.IDisposable {
  private Fixture _fixture = default!;
  private Mock<IAppRepo> _appRepo = default!;
  private Mock<IGameRepo> _gameRepo = default!;
  private Mock<GameLogic> _logic = default!;
  private GameLogic.IBinding _binding = default!;
  private Game _game = default!;

  [SetupAll]
  public void SetupAll() {
    // Any global setup, e.g. serialization, can go here
  }

  [Setup]
  public async Task Setup() {
    _fixture = new(TestScene.GetTree());
    _appRepo = new();
    _gameRepo = new();
    _logic = new();
    _binding = GameLogic.CreateFakeBinding();

    _logic.Setup(l => l.Bind()).Returns(_binding);

    _game = new Game {
      GameRepo = _gameRepo.Object,
    };

    // Inject dependencies if needed
    (_game as IAutoInit).IsTesting = true;
    _game.FakeDependency(_appRepo.Object);

    await _fixture.AddToRoot(_game);
  }

  [Cleanup]
  public async Task Cleanup() => await _fixture.Cleanup();

  [Test]
  public void Initializes() {
    _game.ShouldNotBeNull();
    _game.Setup();
    _game.OnResolved();
    // Check that dependencies are provided
    (_game as IProvider).ProviderState.IsInitialized.ShouldBeTrue();
  }

  [Test]
  public void HandlesPause() {
    _game.OnResolved();
    var tree = TestScene.GetTree();
    tree.Paused.ShouldBeFalse();
    _game.Call("OnOutputSetPauseMode", true);
    tree.Paused.ShouldBeTrue();
    _game.Call("OnOutputSetPauseMode", false);
    tree.Paused.ShouldBeFalse();
  }

  [Test]
  public void HandlesVisibility() {
    _game.OnResolved();
    _game.Visible.ShouldBeTrue();
    _game.Call("OnOutputUpdateVisibility", false);
    _game.Visible.ShouldBeFalse();
    _game.Call("OnOutputUpdateVisibility", true);
    _game.Visible.ShouldBeTrue();
  }

  [Test]
  public void HandlesInputEvents() {
    _game.OnResolved();
    var pauseEvent = new InputEventAction { Action = "HardCancel", Pressed = true };
    var saveEvent = new InputEventAction { Action = "Quicksave", Pressed = true };
    var loadEvent = new InputEventAction { Action = "Quickload", Pressed = true };
    _game._UnhandledInput(pauseEvent);
    _game._UnhandledInput(saveEvent);
    _game._UnhandledInput(loadEvent);
    // No exceptions = pass
  }

  [Test]
  public void OnExitTree_StopsLogicAndDisposesBinding() {
    _game.OnResolved();
    _game.OnExitTree();
    // No exceptions = pass
  }

  public void Dispose() {
    _game?.OnExitTree();
    _binding?.Dispose();
    _game = null!;
    _logic = null!;
    _appRepo = null!;
    _gameRepo = null!;
    _binding = null!;

    GC.SuppressFinalize(this);
  }

}
