# RotateBoomArmTest01

Unityで重機（ユンボ）のブームとアームを、それぞれの接続部を回転軸にして操作するサンプルプロジェクトです。

## 環境

- Windows 11
- Unity 6.4（テンプレート：Universal 3D）
- Input System（新InputSystem）

## 操作方法

JIS式レバー操作をキーボードで再現しています。

| キー | 操作 |
|------|------|
| Q | Arm Dump（アームを伸ばす） |
| A | Arm Curl（アームを曲げる） |
| E | Boom Down（ブームを下げる） |
| D | Boom Up（ブームを上げる） |
| S | Reset（初期位置に戻す） |

## ヒエラルキー構造

```
GameScene
└── BodyRoot（Empty・Scale 1,1,1）
    ├── Body（車体・見た目専用）
    ├── BoomAxis（Empty・ブームの回転軸）
    └── BoomRoot（Empty・Scale 1,1,1）
        ├── Boom（ブーム・見た目専用）
        ├── ArmAxis（Empty・アームの回転軸）
        └── Arm（アーム）
```

## 実装のポイント

### 1. 透明なEmptyオブジェクトを回転軸マーカーとして使う

`RotateAround()` はワールド座標を回転軸として受け取ります。  
BoomAxis・ArmAxisをそれぞれの接続部に配置し、その座標を回転軸として参照することで、端点を軸にした回転を実現しています。

```csharp
boom.RotateAround(boomAxis.position, Vector3.forward, rotateSpeed * Time.deltaTime);
```

回転軸マーカー（BoomAxis等）は「座標を貸しているだけ」で、回転の主役はあくまでBoom・Armです。

### 2. 見た目用オブジェクトを兄弟として分離する

Unityでは**祖先オブジェクトのスケールが非均一**（例：Scale 2,1,1）だと、子オブジェクトが `RotateAround()` で回転したときに**歪みや軸ズレが発生**します。

これを解消するため、以下の構造を採用しました。

- **Rootオブジェクト（Scale 1,1,1）** を制御の基準点として置く
- **見た目用オブジェクト（Bodyなど）** はRootの子として配置、子オブジェクトと**兄弟**の関係になる
    - 例えばBodyはBodyRootの子として配置、その結果、BoomRoot,BoomAxisと兄弟になる
    - これで見た目のスケールが自由に調整でき、祖先オブジェクトのスケールは(1,1,1)が保持されて回転への悪影響がなくなる

### 3. リセット処理

`Start()` で初期位置・回転を保存しておき、Sキー押下時に `SetPositionAndRotation()` で一括復元します。

```csharp
void Start()
{
    initBoomPos = boom.position;
    initBoomRot = boom.rotation;
    initArmPos = arm.position;
    initArmRot = arm.rotation;
}
```

## スクリプト

- `ExcavatorController.cs` をExcavatorController（Empty）にアタッチ
- InspectorでBoom, BoomAxis, Arm, ArmAxisをアタッチ

※注意：Boomには「BoomRoot」をアタッチする→親子関係の親をアタッチしないと子が分離するため

<img width="460" height="222" alt="image" src="https://github.com/user-attachments/assets/7afd7b91-7571-4e9d-8895-b985565221d4" />


## 関連記事

- [Unityで横長Cubeの左端、右端を回転軸にして回転する操作](https://note.com/inu94suno/n/n605e5fc46164)
