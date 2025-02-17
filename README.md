# Toggle Boolean(Switch) Extension for Visual Studio 2022

## How to run

1. Move cursor to a boolean text
2. Tools - Switch / Switch Reverse

## List of boolean values

```
true -> false
yes -> no
on -> off
0 -> 1
&& -> ||
publish -> internal -> private
```

## Keybinding

You can set the shortcuts:

* `Alt+B` to `Tools.Switch` command
* `Alt+Shift+B` to `Tools.SwitchReverse` command

If VsVim, add follow to `.vsvimrc`

```
noremap <Space>i :vsc Tools.Switch<CR>
noremap <Space>I :vsc Tools.SwitchReverse<CR>
```

## Build

Build `Release` then get `ToggleBoolean\bin\Release\ToggleBoolean.vsix`

## Reference

* [Toggle Boolean](https://marketplace.visualstudio.com/items?itemName=silesky.toggle-boolean)
* [AndrewRadev/switch.vim](https://github.com/AndrewRadev/switch.vim)
