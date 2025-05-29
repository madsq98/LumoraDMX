# LumoraDMX

LumoraDMX is a modern, cross-platform DMX lighting control application built using Avalonia UI and .NET 8. It provides an intuitive UI for designing and triggering lighting scenes, and a robust backend for managing DMX output and project data.

## Features

- ✨ Beautiful, responsive UI using Avalonia  
- 🔹 Modular architecture with separate frontend and backend projects  
- ✨ Real-time DMX channel control (supports up to 512 channels)  
- 🌟 Channel sliders with live values and customizable layout  
- 🛋️ Project management and persistence  
- 🧼 Backend auto-hosted alongside frontend in single `.exe`  
- ✉️ mDNS service announcement for network discovery  
- 🎧 Future-ready: line-in BPM detection and beat-reactive lighting planned!

## Technologies

- [.NET 8](https://dotnet.microsoft.com/)
- [Avalonia UI](https://avaloniaui.net/)
- [Entity Framework Core (SQLite)](https://learn.microsoft.com/en-us/ef/core/)
- [Makaretu.Dns](https://github.com/richardschneider/net-mdns) for mDNS

## Project Structure

<pre>
LumoraDMX/
├── DesktopApplication/     # Avalonia frontend
├── Backend/                # .NET Web API backend
├── FrontendServices/       # Shared frontend service interfaces
├── Infrastructure/         # EF Core database context and models
├── MQDmxController/        # DMX output abstraction and implementation
</pre>

## Running the App

To run both the frontend and backend together:

1. **Start the `DesktopApplication` project**  
   This bootstraps the Avalonia UI **and** hosts the backend Web API inside the same process.

2. The backend is started on a **dynamic port** and bound to `0.0.0.0`. It advertises itself via mDNS.

## Publishing

1. Right-click `DesktopApplication` in Visual Studio  
2. Click `Publish`  
3. Use folder target, `win-x64`, self-contained  
4. The backend is bundled automatically due to `ProjectReference`

## Sliders and Channel Control

- Each tab supports up to 128 channels (4 tabs total for 512 channels)  
- Sliders are grouped using a `UniformGrid` (32 per row)  
- Each slider reflects real DMX channel values  
- Value changes trigger `SimpleDmxService.SetDmxChannel(channel, value)`

## Future Plans

- 🕐 Beat/tempo detection via line-in using NAudio or Aubio  
- ✨ Scene sequencing and playback  
- 🌆 Live sound-reactive lighting engine  
- 🔺 MIDI and OSC input support

## Contributing

PRs are welcome! If you'd like to add device support, audio analysis, or help improve UI/UX, open an issue or fork the repo.

## License

MIT License — see `LICENSE` file for details.

---

Made with ❤️ by [@madsq98](https://github.com/madsq98)
