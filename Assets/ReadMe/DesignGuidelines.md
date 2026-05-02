# Design Guidelines

## 設計思想
クリーンアーキテクチャをベースにした階層構造の設計です。
現状はリリース予定のないあくまで個人製作作品ですが、ゲームの内部機能の肥大化が予想されたため、
各役割の明確化と、コードを人に見てもらうことを考慮して可読性の向上を目的としてこの設計を採用しています。
現段階で若干のオーバーエンジニアリングは発生してしまいますが、僕の趣味と勉強もかねて時間の許す限りで
作り続けたいと考えているため、将来的な拡張性を考慮してこの設計を採用しています。

## 階層構造
各階層の説明

### View
 - View : ユーザーインターフェースを表すクラス

### Application
 - Controller : Viewからのリクエストを受け取り、UseCaseを呼び出すクラス
 - Presenter : UseCaseからのレスポンスを受け取り、Viewに渡すクラス
 - Application : UseCaseを実装するクラス

### Domain
 - DomainService : PureC#による内部ロジックの実装を行う
 - Data : PureC#によるデータ構造を定義するクラス
 - RepositoryInterface : Dataを取り扱うリポジトリクラスのインターフェース

### Infrastructure
 - Factory : オブジェクトの生成を行うクラス
 - Repository : データベースや外部APIとのやり取りを行うクラス

### Composition
 - Initializer : アプリケーションのエントリーポイントとなるクラス

## 依存方向
- Domain
- Application
- View
- Infrastructure