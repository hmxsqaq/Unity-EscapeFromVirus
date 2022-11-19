# 技术文档

**项目名：**躲避病毒？

**使用引擎：**Unity

**引擎版本：**2020.3.41f1c1 LTS

## 待完成

GameOver逻辑编写

结尾场景搭建

答题系统

***

## 玩家移动

##### 提供接口：

PlayerSpeed—决定玩家速度

OperationRange—决定可操作范围

##### 实现方案与思路：

采用了Unity的新Input System系统使其同时适配PC上的鼠标点击与手机触屏操作

在Input System中建立了Position与Act两个Action

当玩家点击鼠标左键即会触发Act的回调函数，玩家拖动鼠标则会触发Position的回调函数

Position与Act的回调函数都包含一个bool变量

**状态1：**鼠标左键未被按下时，bool值为false，此时Position的回调函数不对鼠标位置进行监听

**状态2：**鼠标左键被按下时，bool值被设置为true，Position的回调函数开始监听并返回鼠标坐标，同时获取玩家坐标，这两个向量相减即可获得速度向量，使玩家向速度向量方向移动即可实现”拖拽“效果

**状态3：**鼠标左键被抬起，bool值被重新设置为false，回到状态1

***

## Element类

##### 提供接口(Prefabs中修改)：

ElementSpeed—Element移动速度

Damage—Element与玩家碰撞时时所造成的伤害(当为负值时表现为治疗)

##### 实现方案与思路：

游戏中除玩家外其余动态精灵全部继承自一个Element基类

Element基类实现了获取玩家当前位置，并向玩家位置飞去的功能

同时提供了Speed与Damage两个接口，分别用于设置精灵的速度和与玩家发生碰撞时造成的伤害

当Element与玩家碰撞或飞出相机外时自动销毁

***

## Element Generator

##### 提供接口：

Harmful/Beneficial List—分别存放负面/正面的Prefabs

Harm Probability—代表生成负面Prefabs的概率，为1时代表只会生成负面Prefabs

Difficulty List—代表生成Prefabs的时间间隔，每隔一分钟List的index+1(待改进)

##### 实现方案与思路：

通过协程每隔一段时间实例化一个Prefabs

实例化的对象与位置通过随机数决定

位置的随机方法采取了角度随机，随机一个角度值，对象将会再以画面中心为圆心，一个固定值为半径的圆周上随机生成

***

## 定时换口罩系统

##### 提供接口：

MinTriggerTime—两次换口罩玩法时间间隔的下界

MaxTriggerTime—两次换口罩玩法时间间隔的上界

ExchangeTime—玩家需在多少秒内完成换口罩操作

##### 实现方案与思路：

通过两个协程的互相循环调用实现了定时换口罩玩法

游戏开始时启动协程A，协程A会在[MinTriggerTime，MaxTriggerTime)中给出一个随机整数作为下次换口罩玩法的开启倒计时，倒计时结束后，协程A启动协程B，关闭自己

协程B接受一个整形参数ExchangeTime作为时间限制，通过一个bool值来控制内部的while函数，协程B启动时，设置该布尔值为真，后开始while循环，每轮循环持续1s，使ExchangeTime-1，当ExchangeTime<0时游戏结束；当玩家与右下角的Trigger碰撞时，bool值被设定为假，跳出while循环，协程B开启协程A，关闭自己

***

## 答题系统

**提供接口**：



**实现方案与思路**：



***