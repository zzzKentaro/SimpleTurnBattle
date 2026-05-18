\# 2Dターン制バトルテンプレート



Unity初心者向けの、1対1のシンプルな2Dターン制バトルテンプレートです。



プレイヤーと敵がそれぞれ技を1つ選び、素早さの高い方から順番に行動します。  

技によって相手にダメージを与えたり、自分を回復したりできます。  

どちらかのHPが0になるとバトル終了です。



このテンプレートは、完成品のゲームではなく、Unity初心者が



\- ターン制バトルの基本構造を学ぶ

\- ScriptableObjectでデータ管理を学ぶ

\- enumによる状態管理を学ぶ

\- コルーチンによる時間差演出を学ぶ

\- 自分なりに改造してゲームを広げる



ことを目的としています。



\---



\## ゲーム内容



プレイヤーキャラクターと敵キャラクターが1対1で戦います。



プレイヤーは毎ターン、最大6つの技から1つを選びます。  

敵は覚えている技の中からランダムで1つを選びます。



その後、プレイヤーと敵の素早さを比較し、素早い方から行動します。  

同じ素早さだった場合は、ランダムで先攻が決まります。



先攻の攻撃で相手のHPが0になった場合、後攻は行動せず、その時点で勝敗が決まります。



\---



\## 必要なUnity環境



\- Unity 2022以降を想定

\- 2Dプロジェクト

\- TextMeshProを使用



初回起動時にTextMeshProのインポートを求められた場合は、  

`Import TMP Essentials` を実行してください。



\---



\## スクリプト一覧



このテンプレートでは、以下のスクリプトを使用します。



```text

BattleElement.cs

ElementReaction.cs

SkillKind.cs

SkillTarget.cs

BattleState.cs



ElementAffinity.cs

CharacterData.cs

SkillData.cs



BattleActor.cs

DamageResult.cs

DamageCalculator.cs



BattleCharacterView.cs

SkillButtonUI.cs

BattleLogUI.cs

BattleManager.cs





各スクリプトの役割

BattleElement.cs



技の属性を定義するenumです。



Fire

Water

Wind

Earth

Light

Dark



火、水、風、土、光、闇の6属性を扱います。



ElementReaction.cs



属性相性を定義するenumです。



Weak    弱点

Normal  普通

Resist  耐性

Null    無効



キャラクターごとに、各属性に対する相性を設定できます。



SkillKind.cs



技の種類を定義するenumです。



Attack  攻撃技

Heal    回復技

SkillTarget.cs



技の対象を定義するenumです。



Enemy   敵を対象にする

Self    自分を対象にする

BattleState.cs



バトルの現在状態を管理するenumです。



Start

PlayerCommand

EnemyCommand

ActionOrderDecide

PlayerAction

EnemyAction

DamageProcess

CheckBattleEnd

Win

Lose



「今はプレイヤーが技を選ぶ時間なのか」

「今は攻撃演出中なのか」

「今は勝敗判定中なのか」



といった状態を分かりやすく管理するために使います。



ElementAffinity.cs



キャラクターごとの属性相性を表すクラスです。



たとえば、ある敵に対して



Fire  : Weak

Water : Resist

Dark  : Null



のように設定できます。



CharacterData.cs



キャラクターのデータを管理するScriptableObjectです。



設定できる内容は以下です。



キャラクター名

表示用画像

最大HP

攻撃力

防御力

素早さ

属性相性

覚えている技リスト



プレイヤーや敵のデータは、この CharacterData を作って設定します。



SkillData.cs



技のデータを管理するScriptableObjectです。



設定できる内容は以下です。



技名

技アイコン

技の種類

対象

属性

威力

命中率

エフェクトPrefab

エフェクト削除時間



攻撃技や回復技は、この SkillData を作って設定します。



BattleActor.cs



バトル中のキャラクターの状態を管理するクラスです。



CharacterData は元データですが、実際のバトル中にはHPが減ったり回復したりします。

そのため、現在HPなどの「バトル中に変化する値」は BattleActor で管理します。



DamageResult.cs



ダメージ計算の結果を入れるためのクラスです。



命中したか

無効だったか

与えたダメージ量

属性相性



などをまとめて保持します。



DamageCalculator.cs



ダメージ計算を行うクラスです。



攻撃技のダメージは、基本的に以下の式で計算します。



基本ダメージ = 技の威力 + 攻撃側の攻撃力 - 防御側の防御力

最終ダメージ = 基本ダメージ × 属性相性倍率



属性相性倍率は以下です。



Weak   : 1.5倍

Normal : 1.0倍

Resist : 0.7倍

Null   : 0倍



無効以外の場合、最低ダメージは1になります。



BattleCharacterView.cs



キャラクターの見た目を管理するスクリプトです。



担当する表示は以下です。



キャラクター画像

名前

HPテキスト

HPバー

被ダメージ時の点滅

HPバーのアニメーション

エフェクト生成位置



バトルの計算そのものではなく、画面表示を担当します。



SkillButtonUI.cs



技ボタンを管理するスクリプトです。



技名やアイコンを表示し、クリックされたときに BattleManager へ選択内容を伝えます。



BattleLogUI.cs



バトルログを表示するスクリプトです。



主人公の ファイア！

敵に 25 ダメージ！

弱点！



のようなメッセージを画面に表示します。



BattleManager.cs



バトル全体を管理する中心スクリプトです。



主な役割は以下です。



バトル開始処理

プレイヤーの技選択

敵の技選択

行動順の決定

技の実行

ダメージ処理

HPバー更新

勝敗判定

次ターンへの移行



このテンプレートの中心になるスクリプトです。



シーンの作り方



Hierarchyに以下のような構成を作ります。



Canvas

├── PlayerPanel

│   ├── PlayerImage

│   ├── PlayerNameText

│   ├── PlayerHPText

│   └── PlayerHPFill

│

├── EnemyPanel

│   ├── EnemyImage

│   ├── EnemyNameText

│   ├── EnemyHPText

│   └── EnemyHPFill

│

├── CommandPanel

│   ├── SkillButton1

│   ├── SkillButton2

│   ├── SkillButton3

│   ├── SkillButton4

│   ├── SkillButton5

│   └── SkillButton6

│

├── LogText

└── TurnText



BattleManager

UIの設定

PlayerPanel / EnemyPanel



PlayerPanel と EnemyPanel に BattleCharacterView を追加します。



Inspectorで以下を設定します。



Character Image

Name Text

HP Text

HP Fill Image

Effect Point



HPバーに使うImageは、以下のように設定してください。



Image Type  : Filled

Fill Method : Horizontal

SkillButton



各技ボタンに SkillButtonUI を追加します。



Inspectorで以下を設定します。



Button

Skill Name Text

Skill Icon Image



技アイコンを使わない場合は、Skill Icon Image は空でも構いません。



BattleManager



空のGameObjectを作り、名前を BattleManager にします。

そこに BattleManager.cs を追加します。



Inspectorで以下を設定します。



Player Data

Enemy Data



Player View

Enemy View



Skill Buttons

Battle Log

Turn Text



Skill Buttons には、6個の技ボタンを順番に登録します。



ScriptableObjectの作り方



Projectビューで右クリックして、以下を選びます。



Create > TurnBattle > Character Data

Create > TurnBattle > Skill Data

SkillDataの設定例

ファイア

Skill Name : ファイア

Skill Kind : Attack

Target     : Enemy

Element    : Fire

Power      : 20

Accuracy   : 95

アクア

Skill Name : アクア

Skill Kind : Attack

Target     : Enemy

Element    : Water

Power      : 18

Accuracy   : 100

ヒール

Skill Name : ヒール

Skill Kind : Heal

Target     : Self

Element    : Light

Power      : 25

Accuracy   : 100

CharacterDataの設定例

プレイヤー

Character Name : 主人公

Max HP         : 120

Attack         : 15

Defense        : 8

Speed          : 12



属性相性の例です。



Fire  : Normal

Water : Resist

Wind  : Normal

Earth : Weak

Light : Normal

Dark  : Null



覚えている技リストに、作成した SkillData を最大6個登録します。



敵

Character Name : スライム

Max HP         : 100

Attack         : 12

Defense        : 5

Speed          : 8



属性相性の例です。



Fire  : Weak

Water : Normal

Wind  : Normal

Earth : Resist

Light : Normal

Dark  : Normal



敵も覚えている技リストに SkillData を登録します。

敵はこのリストの中からランダムで技を選びます。



バトルの流れ



このテンプレートのバトルは、以下の流れで進みます。



1\. バトル開始

2\. プレイヤーと敵のHPを表示

3\. プレイヤーが技を選ぶ

4\. 敵がランダムに技を選ぶ

5\. 素早さを比較して行動順を決める

6\. 速い方が技を使う

7\. ダメージまたは回復処理を行う

8\. HPバーを更新する

9\. 戦闘不能か確認する

10\. 勝敗が決まっていなければ、遅い方が技を使う

11\. もう一度、HPバー更新と戦闘不能判定を行う

12\. 勝敗が決まっていなければ次のターンへ進む

ダメージ計算



攻撃技のダメージは、以下の流れで計算します。



1\. 命中判定

2\. 属性相性を確認

3\. 基本ダメージを計算

4\. 属性倍率をかける

5\. 最終ダメージを決定



計算式は以下です。



基本ダメージ = 技の威力 + 攻撃側の攻撃力 - 防御側の防御力

最終ダメージ = 基本ダメージ × 属性相性倍率



ただし、属性が無効の場合はダメージ0になります。

無効でない場合、最低ダメージは1です。



回復技について



回復技は、自分または対象のHPを回復します。



このテンプレートでは、回復量を以下の式で計算しています。



回復量 = 技の威力 + 使用者の攻撃力



HPは最大HPを超えて回復しません。



コルーチンについて



ターン制バトルでは、内部処理だけを一瞬で行うと、プレイヤーには何が起きたのか分かりません。



そのため、このテンプレートではコルーチンを使って、



ログを表示する時間

攻撃演出を見る時間

HPバーが減る時間

次の行動までの間



を作っています。



これにより、ターン制バトルらしい「間」が生まれます。



このテンプレートで学べること



このテンプレートでは、以下の内容を学べます。



enumによる状態管理

ScriptableObjectによるデータ管理

Buttonを使ったコマンド選択

コルーチンによる時間処理

HPバーのアニメーション

属性相性を使ったダメージ計算

バトル処理と画面表示の分離



特に重要なのは、

「今どの状態だから、どの入力を受け付けるのか」

を考えて作ることです。



改造案



このテンプレートをもとに、以下のような改造ができます。



敵AIを強化する



現在の敵はランダムに技を選びます。



改造すると、たとえば以下のような敵を作れます。



HPが少なくなったら回復する

プレイヤーの弱点属性を狙う

強い技を低確率で使う

特定ターンだけ必殺技を使う

MPを追加する



技にMPコストを追加すると、よりRPGらしくなります。



追加する項目の例です。



CharacterDataに最大MP

BattleActorに現在MP

SkillDataに消費MP

状態異常を追加する



毒、麻痺、防御ダウンなどを追加できます。



例です。



毒    : ターン終了時にダメージ

麻痺  : 一定確率で行動できない

攻撃力アップ : 数ターン攻撃力が上がる

防御力ダウン : 数ターン防御力が下がる

複数の敵に対応する



現在は1対1ですが、改造すると



プレイヤー1人 vs 敵3体

味方3人 vs 敵3体



のような形式にも発展できます。



ただし、複数キャラクターにすると難易度が上がるため、まずは1対1を理解してから挑戦するのがおすすめです。



勝利後のリザルト画面を作る



勝利後に以下のような画面を表示できます。



勝利！

経験値を手に入れた

お金を手に入れた

次のシーンへ進む

技演出を強化する



SkillData にエフェクトPrefabを設定できるため、技ごとに演出を変えられます。



例です。



炎の技なら炎パーティクル

水の技なら水しぶき

光の技なら発光エフェクト

回復技なら緑色の光

よくあるエラー

ボタンを押しても反応しない



以下を確認してください。



SkillButtonUIがボタンについているか

ButtonがInspectorに設定されているか

BattleManagerのSkill Buttonsに登録されているか

CharacterDataのSkillsにSkillDataが登録されているか

HPバーが動かない



HPバー用のImage設定を確認してください。



Image Type  : Filled

Fill Method : Horizontal



また、BattleCharacterView の HP Fill Image に正しいImageが登録されているか確認してください。



キャラクター画像が表示されない



CharacterData の Character Sprite に画像が設定されているか確認してください。



また、BattleCharacterView の Character Image にImageコンポーネントが登録されているか確認してください。



NullReferenceExceptionが出る



何かの参照がInspectorで設定されていない可能性があります。



特に以下を確認してください。



BattleManagerのPlayer Data

BattleManagerのEnemy Data

BattleManagerのPlayer View

BattleManagerのEnemy View

BattleManagerのSkill Buttons

BattleCharacterView内のUI参照

SkillButtonUI内のButton参照

制作時の考え方



ターン制バトルでは、横スクロールアクションのように毎フレーム自由に入力を受け付けるわけではありません。



大事なのは、



今は何の状態か

その状態で何を表示するか

その状態でどの入力を受け付けるか

次にどの状態へ進むか



を整理することです。



このテンプレートでは、その考え方を学びやすくするために、BattleState というenumで状態を管理しています。



ライセンス・利用について



このテンプレートは学習用です。

授業、講座、個人制作、改造練習などに自由に使用できます。



素材画像や効果音を追加する場合は、それぞれの素材の利用規約を確認してください。



まとめ



このテンプレートは、Unity初心者がターン制バトルの基本構造を理解するための教材です。



最初はそのまま動かし、次に数値や技を変え、慣れてきたら敵AIや状態異常などを追加してみてください。



まずは、



キャラクターを作る

技を作る

ボタンで選ぶ

攻撃する

HPが減る

勝敗が決まる



という基本ループを理解することが目標です。

