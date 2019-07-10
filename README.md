# Constructor

A Starcounter demo application

## Purpose

This demo application demonstrates a fraction of [PALMA solution](https://modularmanagement.com/palma/) developed by [Modular Management](https://modularmanagement.com/) on [Starcounter](https://starcounter.com/).

## Compatibility

Current source code is compatible with the latest (as of 2019.07.08) Starcounter release - [`2.3.2`](https://starcounter.io/download/). Starcounter 3.0 compatible version might come in the future.

## Setup

- Download & install [Visual Studio](https://visualstudio.microsoft.com/downloads/) 2017 or 2019.
- Download & install [Starcounter 2.3.2](https://starcounter.io/download/).
  - Make sure to install Visual Studio extension.
  - Visual Studio 2019 requires manual [extension](https://marketplace.visualstudio.com/items?itemName=Starcounter.StarcounterforVisualStudio) installation.
- Clone source code.
- Open `~/Constructor/Constructor.sln` solution in Visual Studio.
- Build solution - `Ctrl + Shift + B`.
- Start `Constructor` project - `Ctrl + F5`.
- Navigate to `http://localhost:8080/constructor` with [Opera](https://www.opera.com/computer) or [Google Chrome](https://www.google.com/chrome/).

**Note:** other browsers than Google Chrome might work as well, but were not tested.

## Entry point

The application has two entry points:

 - `/constructor` - home page.
 - `/constructor/product/?` - product details page, where `?` is numeric object number of a `Constructor.Database.Product` instance.

 ## Demo data

 See the [`DEMO.md`](DEMO.md) file for some demo data, the demo pictures available in the `~/Constructor/Constructor/wwwroot/Constructor/images` folder.

 ## Report an issue

 - Please report any Starcounter related issues at [`Starcounter/Home`](https://github.com/Starcounter/Home/issues) issue tracker.
 - Please report any Demo related issues in this repository issue tracker.

 ## Limitation & Performance

 - This demo app is designed for a single user usage.
 - This demo app is not optimized and shall not be used for any benchmarking or performance references.
 
 ## Screenshots
 
 <details>
  <summary>Click to expand</summary>

### Home page

![image](https://user-images.githubusercontent.com/6435556/60992658-3ba52a00-a34d-11e9-8f17-c61b982bdd39.png)

### Product page - editing

![image](https://user-images.githubusercontent.com/6435556/60992792-7f982f00-a34d-11e9-8459-ec1d88f485f9.png)

### Product page - viewing

![image](https://user-images.githubusercontent.com/6435556/60992827-96d71c80-a34d-11e9-91fc-facfdb35e63b.png)

</details>
