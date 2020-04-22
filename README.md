# TouchKB

A Winform touch keyboard that automatically supports multiple international layouts

## Getting Started

It is a component for Windows written in C # that allows you to edit a text field using the mouse, a touch panel or the keyboard. This touch keyboard automatically changes the layout following the one set in the operating system. In the images below it is possible to view some keyboard layouts.

Italian

<img src="https://i.ibb.co/gFGYP2x/IT-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="IT Layout">


French

<img src="https://i.ibb.co/09tz4YM/FR-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="FR Layout">


German

<img src="https://i.ibb.co/b7njyHF/DE-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="DE Layout">


Spanish

<img src="https://i.ibb.co/Rptt78C/ES-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="ES Layout">


Obviously almost all layouts managed by Windows are supported. 

Since not all touch screens support multiple touch points, this component is capable of handling keystrokes to obtain code-points such as capital letters, numbers or punctuation symbols. For example pressing the [Shift] key the keyboard layout changes like this:

<img src="https://i.ibb.co/7CRNpty/SHIFT-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="Shift key pressed Layout">

Also the [Alt-Gr] key alters keyboard layout:

<img src="https://i.ibb.co/ryWDGCR/ALT-GR-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="Alt-Gr key pressed Layout">

Control key combinations such as [Alt-Gr] + [Shift] are also managed:

<img src="https://i.ibb.co/c2jq9Bn/COMBO-D3-F1-B31-F-621-B-4604-A17-F-C5-A9746958-F9.png" alt="Control key combo pressed Layout">

### Prerequisites

Visual Studio is required: it should also work with older versions than VS2013.

### Installation, Deployment and Usage

Just open the project in visual studio. Compile it and run to test the component. Next you can use the generated assembly or directly the component in your code.

The resulting text is in the component's Text property.

## License

This project is licensed under the "The Unlicense".
