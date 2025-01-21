# Toggle Boolean Extension for Visual Studio 2022

## How to run

1. Move cursor to a boolean text
2. Tools - Toggle Boolean

## List of boolean values

```
true <-> false
yes <-> no
on <-> off
0 <-> 1
```

## Keybinding

You can set the shortcut `Alt+B` to `Tools.ToggleBoolean` command.

If VsVim, add follow to `.vsvimrc`

```
noremap <Space>i :vsc Tools.ToggleBoolean<CR>
```

## Reference

https://marketplace.visualstudio.com/items?itemName=silesky.toggle-boolean
