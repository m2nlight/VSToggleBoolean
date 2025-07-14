# Toggle Boolean(Switch) Extension for Visual Studio 2022

## How to run

1. Move cursor to a boolean text
2. Tools - Switch / Switch Reverse

## List of boolean values

```
true -> false
on -> off
yes -> no
0 -> 1
&& -> ||
private -> public
internal -> public
class -> struct -> enum -> record
```

## Keybinding

You can set the shortcuts:

* `Alt+B` to `Tools.Switch` command
* `Alt+Shift+B` to `Tools.SwitchReverse` command

If VsVim, add follow to `.vsvimrc`

```
noremap <Space>y :vsc Tools.Switch<CR>
noremap <Space>Y :vsc Tools.SwitchReverse<CR>
```

## Build

```ps1
# PowerShell
.\build.ps1
```

Build `Release` then get `ToggleBoolean\bin\Release\ToggleBoolean.vsix`

## Reference

* [Toggle Boolean](https://marketplace.visualstudio.com/items?itemName=silesky.toggle-boolean)
* [AndrewRadev/switch.vim](https://github.com/AndrewRadev/switch.vim)
