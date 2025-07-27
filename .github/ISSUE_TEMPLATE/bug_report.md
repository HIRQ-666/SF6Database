name: バグ報告
description: 発生した不具合を記録するテンプレート
title: "[Bug] "
labels: [bug]
body:
  - type: textarea
    id: summary
    attributes:
      label: 概要
      description: 何が問題か簡単に書いてください
      placeholder: 例）アプリが起動時にクラッシュします
  - type: textarea
    id: steps
    attributes:
      label: 再現手順
      description: バグの再現方法（手順）を記入してください
      placeholder: |
        1. アプリを起動
        2. 「開始」ボタンをクリック
        3. エラーが表示される
  - type: input
    id: env
    attributes:
      label: 使用環境（任意）
      placeholder: Windows 11, .NET 8, Visual Studio 2022
