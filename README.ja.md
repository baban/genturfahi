# genturfa'i

* [[English Help(README.md)](README.md)

<table>
  <tr>
    <th>ソフト名前</td>
    <td>「genturfa'i」</td>
  </tr>
  <tr>
    <th>バージョン</td>
    <td>ソフト起動後「バージョン情報」を参照してください</td>
  </tr>
  <tr>
    <th>種別</td>
    <td>フリーソフト</td>
  </tr>
  <tr>
    <th>作者名</td>
    <td>baban</td>
  </tr>
</table>

ファイルは下のURLで公開されています。

 * http://babanba-n.iobb.net/file/genturfahi.html

![alt screenshot](/image/screeenshot.ja.png)

ソースコードはgithub上で公開されています。

[https://github.com/baban/genturfahi](https://github.com/baban/genturfahi)

## これは何？

人口言語ロジバンの構文解析を行うパーサーです。

「Official LLG Parser」をWindowsのアプリケーションから簡単に使えるフロントエンドとして実装されています。

 * [Official LLG Parser](https://mw.lojban.org/papri/Official_LLG_Parser)
 * [Wikipedia ロジバン](https://ja.wikipedia.org/wiki/ロジバン)
 * [はじめてのロジバン](http://seesaawiki.jp/hajiloji/)

## 動作環境

 * .Net Framework4.5以上がインストールされているWindowsマシン
 * 動作確認はWindows10で行っていますが、WindowsVista以降であれば問題なく動くはずです。

## 免責

動作確認には細心の注意を払っていますが、このソフトを使ったことによる
いかなる損害に対しても、作者は責任を負う事は出来ません
ご使用については自己責任の下でお願いいたします。

## インストール・アンインストール

### インストール

インストール作業は必要ありません。
解凍されたzipファイルの中、手元の「genturfahi.exe」をそのまま実行すればアプリケーションが立ち上がります。

### アンインストール

いらなくなったら、解凍時に出来たフォルダをそのまま削除していただければ大丈夫です。

## 使い方

アプリケーションを起動したら、画面上のテキストボックスにロジバン文を入力してください。
解析結果が、下のテキストボックスに表示されます。

解析結果の見方ですが

デフォルトの例文の「coi」を解析すると、「({coi DO'U} /FA'O/)」として帰ってきますが、"DO'U"や"FA'O"は
構文として省略可能な文字を追加した形で結果を返します。
ですので「coi do'u」等省略できるものをあえて追加しても
「coi」の時とほぼ同じ解析結果を返します。

パースに失敗すると、赤文字でエラーが起きた行番号を返します。

## ロジバン例文集

<dl>
  <dt>coi</dt>
  <dd>訳：Hello</dd>
  <dt>ti penbi</dt>
  <dd>訳：これはペンです</dd>
  <dt>mi prami du</dt>
  <dd>訳：I love you.</dd>
  <dt>le ninmu cu melbi</dt>
  <dd>訳：その女性は美しい。</dd>
</dl>

## 開発環境、謝辞

このソフトはVisual Studio 2013で開発をしています。
