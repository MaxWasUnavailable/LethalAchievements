# Lethal Achievements API

*A framework for creating and managing achievements in Lethal Company.*

[![Build](https://img.shields.io/github/actions/workflow/status/MaxWasUnavailable/LethalAchievements/build.yml?style=for-the-badge&logo=github&branch=master)](https://github.com/MaxWasUnavailable/LethalAchievements/actions/workflows/build.yml)
[![Latest Version](https://img.shields.io/thunderstore/v/MaxWasUnavailable/LethalAchievements?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/lethal-company/p/MaxWasUnavailable/LethalAchievements)
[![Thunderstore Downloads](https://img.shields.io/thunderstore/dt/MaxWasUnavailable/LethalAchievements?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/lethal-company/p/MaxWasUnavailable/LethalAchievements)
[![NuGet Version](https://img.shields.io/nuget/v/MaxWasUnavailable.LethalAchievements?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/MaxWasUnavailable.LethalAchievements)

## What is this?

This is a framework for creating achievements in Lethal Company. It is designed to be simple to use, while also being
flexible enough to allow mostly any kind of achievement to be created.

The library makes use of [LethalModDataLib](https://github.com/MaxWasUnavailable/LethalModDataLib) to keep track of
whether an achievement was completed or not. It is recommended to use this library to keep track of your achievement
internal state (e.g. progress towards completion).

## How to use

### Creating an achievement

An achievement needs to be a class that implements the `IAchievement` interface. Additionally, the achievement should
evoke the `AchievedEvent` event whenever it is completed, since the achievement manager listens to this event.

It is, however, recommended to use the `BaseAchievement` class as a base class for your achievement. This class already
provides a simple `Complete()` method that you can use to mark the achievement as completed.

Included in things you need to implement are the `Name` and `DisplayText` properties, which are used for the achievement
popup. There is also the `SaveLocation` property, which decides whether the achievement is a global achievement or a
save-specific achievement (This means the achievement can either be achieved once for all saves, or once for each save).
Make sure to also set the `IsAchieved` property to `false`, since otherwise the achievement will be considered achieved
by default.

Finally, you'll need to implement the `Initialize` and `Uninitialize` methods. You should use these methods to subscribe
and unsubscribe from events that your achievement listens to, as well as clear any internal state that you might have
that shouldn't carry over between game loads. The `Initialise` method is called by the achievement manager on game load.
Additionally, when an achievement is achieved, the `Uninitialize` method is called, and the achievement will be skipped
in the future for that save (assuming the `SaveLocation` is set to `SaveLocation.CurrentSave`, and the save is not
deleted).

> [!IMPORTANT]
> 
> Achievement names should be unique inside of a single plugin. If you have multiple achievements with the same name,
> the achievement manager will only keep track of the first one it encounters.
> 
> Achievement names do not need to be unique across plugins. If you have multiple plugins with achievements that have
> the same name, the achievement manager will keep track of all of them. Internally, it prefixes the achievement name
> with the plugin's name to ensure uniqueness.

### Registering an achievement

To register an achievement, you need to instantiate it and register it with the achievement manager. The achievement
manager will handle keeping track of, loading, and saving the achievement's IsAchieved state. It will initialize the
achievement when a game is loaded. If your achievement uses the `ModData` attribute from
[LethalModDataLib](https://github.com/MaxWasUnavailable/LethalModDataLib) for any of its fields or properties, the
achievement manager will also register the achievement with said library for you, ensuring the achievement's internal
state is saved and loaded correctly.

To register an achievement, all you need to do is call the `AchievementManager.RegisterAchievement` method with an
instance of your achievement class as an argument. This should be done in your plugin's `Awake` method.

### Implementing an achievement

The implementation of the achievement itself is up to you. It is recommended to use the
[LethalModDataLib](https://github.com/MaxWasUnavailable/LethalModDataLib) library to keep track of the achievement's
internal state. It is recommended to use events to change your achievement's internal state, and to let the achievement
itself handle the logic of whether it is completed or not.

### Example

Below is an example of a simple achievement that is completed when the player has jumped 10 times.

It extends the `BaseAchievement` class, since that provides the `Complete()` method. Additionally, it overrides the
Name and DisplayText properties since these are required for the achievement to be displayed in the achievement popup.

It keeps track of the number of times the player has jumped in a field that is saved and loaded by the
[LethalModDataLib](https://github.com/MaxWasUnavailable/LethalModDataLib) library. This ensures progress towards the
achievement is continued between game loads.

Finally, it subscribes to the `PlayerJumpEvent` event in the `Initialize` method, and unsubscribes from it in the
`Uninitialize` method. When the player jumps, it increments the `JumpCount` field, and if the player has jumped 10
times, it calls the `Complete()` method to mark the achievement as completed. The achievement manager will then take
care of the rest *(e.g. displaying the achievement popup, calling the `Uninitialize` method, etc.)*

```csharp
/// <summary>
///     An example achievement that will be achieved when the player jumps 10 times.
/// </summary>
public class Jump10Achievement : BaseAchievement
{
    /// <summary>
    ///     The number of times the player has jumped.
    /// </summary>
    /// <remarks> This field will be saved and loaded by the ModData library. </remarks>
    [ModData(SaveWhen = SaveWhen.OnSave, LoadWhen = LoadWhen.OnLoad, SaveLocation = SaveLocation.CurrentSave)]
    private int JumpCount { get; set; }
    
    /// <inheritdoc />
    public override string Name { get; set; } = "Jump for Joy";
    
    /// <inheritdoc />
    public override string? DisplayText { get; set; } = "You've jumped 10 times!";
    
    /// <inheritdoc />
    public override void Initialize()
    {
        PlayerJumpEvent += OnPlayerJump;
    }

    /// <inheritdoc />
    public override void Uninitialize()
    {
        PlayerJumpEvent -= OnPlayerJump;
    }
    
    private void OnPlayerJump()
    {
        JumpCount++;
        if (JumpCount >= 10)
        {
            Complete();
        }
    }
}
```

## Attribution
<a href="https://www.flaticon.com/free-icon/badge_3179792?term=trophy&related_id=3179792" title="prize icons">Prize icon created by Pixel perfect - Flaticon</a>