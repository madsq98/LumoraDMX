# LumoraDMX

**LumoraDMX** is a modular and extensible DMX512 lighting control system built with .NET, designed for precision, flexibility, and integration.

It features:
- A fast and accurate DMX output engine
- Dynamic fixture management using templates
- Web API interface for external control
- Support for Enttec and FTDI-based USB-DMX interfaces

The project is tested and works with the Eurolite USB-DMX512 PRO Cable interface.
Should work by default with all Enttec USB-DMX512 PRO based interfaces.

---

## 🚀 Features

- 🎛️ **Fixture Templates**: Define fixtures in JSON with named channels (e.g., `Pan`, `Tilt`, `Red`, `Dimmer`).
- 🔌 **Interface Abstraction**: Plug-and-play support for different DMX interfaces via `IDmxOutput`.
- 📡 **Web API**: Control fixtures remotely using a REST API.
- 💾 **EF Core + SQLite**: Projects and fixtures are stored persistently.
- 🧠 **Smooth Animations**: Use `Stopwatch`-timed transitions for flicker-free fades.

---

## 📁 Folder Structure

```
/Controllers       - API endpoints
/Data              - EF Core DbContext
/Models            - Entity and DTO classes
/Services          - Fixture template loading and runtime logic
/FixtureTemplates  - JSON templates for fixture types
```

---

## 🔧 Requirements

- .NET 8 (or .NET 6+)
- FTDI D2XX drivers (for FTDI-based DMX interfaces)

---

## 📦 Getting Started

```bash
# Clone the repo
git clone https://github.com/madsq98/LumoraDMX
cd LumoraDMX

# Run the API
dotnet run --project LumoraDMX
```

### JSON Fixture Template Example

```json
{
  "name": "Cameo_Moonflower_HP",
  "brand": "Cameo",
  "model": "Moonflower HP",
  "mode": "5ch",
  "channels": {
    "ColorWheel": 1,
    "Rotation": 2,
    "Strobe": 3,
    "Dimmer": 4,
    "Macro": 5
  }
}
```

---

## 🧪 Example API Usage

### Set a channel:
```http
POST /api/dmx/channel
{
  "fixtureId": 1,
  "channel": "Dimmer",
  "value": 255
}
```

### Fade a channel:
```http
POST /api/dmx/fade
{
  "fixtureId": 1,
  "channel": "Red",
  "targetValue": 255,
  "duration": 2000
}
```

---

## 🛡 License

This project is licensed under the **GNU GPL v3.0**. See the [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing

Contributions, ideas, and suggestions are welcome! Feel free to fork the repo and open a PR.

---

## 👤 Author

Created by [Mads Qvistgaard](https://github.com/madsq98).
