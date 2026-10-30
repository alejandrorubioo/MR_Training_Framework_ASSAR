# Mixed Reality Training Framework (ASSAR)

Team project built at the ASSAR Industrial Innovation Arena in Skövde, Sweden, in 2022. It was not a university course. I worked across the whole implementation, including the mixed-reality application and the backend. Republished from the team's original repository, which stays private: 433 commits across all branches (June 2022 to March 2023; 431 of them in 2022) by six contributors, listed under [Team and credits](#team-and-credits). This repository is a clean snapshot with one commit and no old history.

> **This project will not open fully as published.** Paid, Asset Store and unclear-licence content has been removed (see [Dependencies not included](#dependencies-not-included)). Scripts will not compile and scenes will show missing references until those dependencies are restored. Read this repository as source code and project structure, not as a ready-to-run build.

## What it is

A Unity framework for training operators in industrial tasks with a Mixed Reality headset. Trainees follow step-by-step instructions with their hands, working on virtual or real objects and alongside a virtual or real collaborative robot. It ships three showcases:

| Scene (`Assets/01 Scenes/Version 2`) | Task |
|---|---|
| `01 HRC Welding` | Human-robot collaborative welding training |
| `02 HRC Battery Assembly` | Battery assembly with a robot handing over parts, including gluing and wiring |
| `03 PLC MBS Assembly` | PLC-driven assembly: HMI buttons, conveyor, gears, screws and drilling |

`00 Start Scene` is the menu. The interface is localised in English and Swedish.

**ASSAR** is the [ASSAR Industrial Innovation Arena](https://assarinnovation.se/en/) in Skövde, Sweden, where the project was built. It is named in the Unity company field and in the `MR_Training_ASSAR` code namespace. This repository is not published by or on behalf of ASSAR. The original images, logos and videos are not included.

## Why it is interesting

- **Real and virtual can be mixed per asset.** Work objects, human tools and robots each switch between real and virtual during a session, and the process logic adapts.
- **New processes are composed, not coded.** A training is a list of steps assembled in the scene from reusable behaviours.
- **It connects to real equipment.** It talks to a PLC over Modbus TCP and to an ABB robot controller over a TCP socket.

## Stack

- Unity 2020.3.31f1, C#
- Varjo XR plugin (mixed reality, depth occlusion, marker tracking)
- Ultraleap hand tracking and Interaction Engine
- game4automation (PLC signal components, drives, robot components)
- EasyModbus (bundled with game4automation)
- Unity Localization, Addressables, TextMesh Pro

## How it works

Scripts are in `Assets/02 Scripts`, in numbered subsystems.

- **Game management (`01`).** `GameManager` is a singleton handling scene changes, reset, quit and language.
- **Process orchestration (`02`).** `Showcase_Manager` owns the list of steps. Each step holds containers of work objects, human tools and robots. The manager starts them, collects `taskFinish` callbacks, advances the step and times each step. User actions are command objects (`Start`, `Next`, `Back`, `Skip`, `Reset`, `Virtuality`, `NewProcess`, `Finished`) run through an `Invoker`. `AssetControl` handles the real/virtual switch for each asset type.
- **Process system (`03`).** `WorkObjectInterface`, `HumanToolInterface` and `RobotInterface` derive from the abstract `ObjectInterface`, which defines how an object answers each command. Reusable behaviours implement the tasks: magnetic placement, wait, welding, screwing and brushing.
- **Communication (`04`).** `TCPTestClient` opens a socket to the robot controller, sends a task message and waits for `done`. It also receives joint values, which `RobotInterface` applies to the virtual robot's axes. `RobotSignalsHandling` drives process signals through PLC components.
- **Instructions (`05`).** Step text is loaded from JSON in `Assets/StreamingAssets/JSON Files`.
- **User interface (`06`).** `UIM` and the menu scripts drive the in-headset menus.
- **VR and MR (`07`).** `MRToggle` and `VRToggle` swap the virtual environment for the real one; `DepthOclussion` handles depth occlusion.
- **Showcase-specific scripts (`09`)** hold task logic such as the battery attach and the drill process. `Old/` keeps earlier code.

The PLC path is in `Assets/01 Scenes/PLC Showcase/Scripts`. `ModbusTest` connects on port 502, writes inputs as coils and reads outputs as discrete inputs, mapped to game4automation PLC signals. `ManualIPCommunication` changes the local Ethernet IP through an elevated `netsh` call, so it needs administrator rights. Its address, mask and gateway come from unsanitised scene fields placed directly in the command, so read it and treat the scenes as trusted input before use. `OpenPLCServer` opens the PLC's reboot URL on a button press.

Lab network addresses in scripts and scenes were replaced by the documentation range `192.0.2.x`; set your own.

## Results

This repository contains no measured evaluation results, so none are claimed. The outcome it documents is the working code and scenes for the three showcases.

## How to run

The project cannot be run from this snapshot alone. To attempt it:

1. Install Unity 2020.3.31f1 and clone this repository.
2. Restore the dependencies below into the listed folders. The `.meta` files of the team's files are preserved, but references to removed assets rely on the GUIDs of the exact versions used, so some will stay broken.
3. Open `Assets/01 Scenes/Version 2/00 Start Scene.unity`.

The real-world features also need a Varjo headset, an Ultraleap sensor, a PLC and an ABB robot controller on the same network. `ProjectSettings/EditorBuildSettings.asset` lists a `LoadingScene` that was already missing from the original repository.

## Dependencies not included

| Dependency | Original location | Where to get it |
|---|---|---|
| game4automation with CADLink, parts4cad, Professional (Asset Store EULA) | `Assets/game4automation` | Unity Asset Store; publisher in2sight GmbH |
| in2sight robot models (ABB, KUKA, SCHUNK, Stäubli) | `Assets/External Assets/in2sight` | in2sight / game4automation |
| Ultraleap Tracking (Apache 2.0; left out for size) | `Assets/External Assets/ThirdParty/Ultraleap` | https://github.com/ultraleap/UnityPlugin |
| Varjo XR plugin | `Packages/manifest.json` | https://github.com/varjocom/VarjoUnityXRPlugin |
| Viveport SDK and native plugins | `Assets/Viveport`, `Assets/External Assets/Plugins` | HTC Viveport developer resources |
| UnityTHX HiS 1.4.0 (provides `HiS.XR.Utils`; no licence found) | `Assets/External Assets/UnityTHX HiS v1.4.0` | Its authors; not publicly sourced here |
| Quick Outline | `Assets/External Assets/QuickOutline` | Its author, Chris Nolet |
| TextMesh Pro resources | `Assets/External Assets/TextMesh Pro` | Unity: TMP Essential Resources importer |
| Factory Interior; HQ Warehouse Kit; 3D Games Effects Pack Free; Welder Set; SNAPS Prototyping Assets | `Assets/External Assets`, `Assets/Objects/Factory` | Unity Asset Store, by name |
| Loading bar sample | `Assets/External Assets/loadingBar` | Source not recorded |
| 3D models, textures and icons of unknown or web origin, including CAD of the assembly parts | `Assets/Objects`, `Assets/Images`, `Assets/01 Scenes/PLC Showcase/Material` | Not available |
| `VarjoMarkerManager.cs` (appears adapted from a Varjo sample) | `Assets/02 Scripts/07 VR and MR` | Varjo marker documentation |
| Showcase videos and photographs | `Assets/Videos`, `Assets/Images` | Not available |

## Repository layout

```
Assets/01 Scenes/Version 2     Final scenes (start, welding, battery, MBS)
Assets/01 Scenes/Version 1     Earlier iteration of the scenes
Assets/01 Scenes/PLC Showcase  PLC scripts, animations and buttons
Assets/02 Scripts              Framework source, by subsystem
Assets/03 Animations           Animation clips and controllers
Assets/Objects, Prefabs, Showcase Creator   Team prefabs
Assets/StreamingAssets         JSON instruction files
Assets/External Assets         Only localisation and Addressables settings
Packages, ProjectSettings      Unity configuration
```

Earlier scene iterations (`Version 0`, `Testing Scenes`) were left out to keep the repository small.

## Team and credits

Contributors to the original repository: [@alejandrorubioo](https://github.com/alejandrorubioo), [@marcosfdez99](https://github.com/marcosfdez99), [@Ensoga](https://github.com/Ensoga), [@pbeigve](https://github.com/pbeigve), [@alexmolera](https://github.com/alexmolera), [@Curr0Jimenez](https://github.com/Curr0Jimenez).

Alejandro Rubio ([@alejandrorubioo](https://github.com/alejandrorubioo)): team project; I worked across the whole implementation, including the mixed-reality application and the backend. The project's in-app credits also list Santiago Molina.

Rights remain with the authors; a licence is pending.
