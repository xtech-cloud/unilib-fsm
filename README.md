# 简介

uniMVCS是一个Unity使用的轻量级的有限状态机框架。

## 框架组成

uniFSM由状态机（Machine）、状态（State）、行为（Action）、命令（Command）四部分组成。

一个状态机由多个状态组成，每个状态包含若干个行为，行为通过命令切换到其他的状态机。

### 状态

### 行为

行为主要有3个方法

- onEnter

当进入一个状态时，会调用当前状态下所有行为的onEnter方法。

- onExit

当离开一个状态时，会调用当前状态下所有行为的onExit方法。

- onUpdate

当一个状态在持续更新时，如果当前状态下的行为并未完成，会调用行为的onUpdate方法。


每个状态包含一个默认的FINISH的行为，当检测到所有行为都完成后，会调用此行为。

# 快速使用

参阅unitysln/UniFSM/Assets/XTC/FSM/Sample/Sample.cs

