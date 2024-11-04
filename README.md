# Sdcb.DashScope ![NuGet](https://img.shields.io/nuget/dt/Sdcb.DashScope.svg?style=flat-square) [![QQ](https://img.shields.io/badge/QQ_Group-495782587-52B6EF?style=social&logo=tencent-qq&logoColor=000&logoWidth=20)](http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=mma4msRKd372Z6dWpmBp4JZ9RL4Jrf8X&authKey=gccTx0h0RaH5b8B8jtuPJocU7MgFRUznqbV%2FLgsKdsK8RqZE%2BOhnETQ7nYVTp1W0&noverify=0&group_code=495782587)

为阿里云灵积模型服务DashScope开发的非官方.NET SDK
![icon](https://raw.githubusercontent.com/sdcb/Sdcb.DashScope/master/icon.png)

## Demo

可以在[这里](https://qwen.starworks.cc:88/)找到一个使用本SDK的在线Demo，源码在[这里](https://github.com/sdcb/Sdcb.DashScope/blob/master/Sdcb.DashScope.Gradio/Program.cs)。

## NuGet包
| Package                                | Version 📌                                                                                                                                                | Description                  |
| -------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------- |
| Sdcb.DashScope                          | [![NuGet](https://img.shields.io/nuget/v/Sdcb.DashScope.svg)](https://nuget.org/packages/Sdcb.DashScope)                                                   | DashScope .NET SDK       |

## 完成情况

* [x] 通义千问 qwen-turbo/plus/max/max-1201/max-longcontext
* [x] 通义千问开源模型 qwen-72b-chat/14b/7b/1.8b
* [x] 通义千问VL qwen-vl-plus
* [x] 其它DashScope中的开源大语言模型如ChatGLM（理论上都可以用）
* [x] Function call
* [ ] 通用文本向量 text-embedding-v1/v2
  * [x] 同步
  * [ ] 异步
* [ ] WordArt锦书
* [x] Stable Diffusion
* [ ] 通义万像
  * [x] 文本生成图像API wanx-v1 
  * [x] 人像风格重绘API wanx-style-repaint-v1
  * [ ] 图像背景生成API wanx-background-generation-v2
* [x] 文件服务
  * [x] 上传
  * [x] 获取列表信息
  * [x] 获取单个文件信息
  * [x] 删除文件
* [x] FaceChain
  * [x] face-detect
  * [x] finetune
  * [x] generation