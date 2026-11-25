<h1 align="center">🔍 Game Inspector</h1>

<p align="center">
  <strong>Thanh công cụ Inspector cho Unity, chỉ chạy trong Editor.</strong>
  <br/>
  Prefab và component thao tác ngay dưới header, không cần mở menu.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2022.3+-000000?logo=unity&logoColor=white&style=flat-square">
  <img src="https://img.shields.io/badge/UPM-Git%20Package-2C8EBB?style=flat-square">
  <img src="https://img.shields.io/badge/Package-com.dann.gameinspector-6A5ACD?style=flat-square">
  <img src="https://img.shields.io/badge/Editor-only-2EA44F?style=flat-square">
  <img src="https://img.shields.io/badge/Owner-PocketPlay-FF6B6B?style=flat-square">
  <a href="CHANGELOG.md"><img src="https://img.shields.io/badge/Changelog-1.0.0-0366D6?style=flat-square"></a>
</p>

---

Package thêm một khu vực dưới header Inspector của GameObject đang chọn. Không có code runtime, không vào build.

**Yêu cầu:** Unity 2022.3 trở lên. Package ID: `com.dann.gameinspector`.

## Bắt đầu nhanh

1. Trong Unity, mở **Window → Package Manager → + → Add package from git URL...** và nhập:

   ```text
   https://github.com/PocketPlay-Interactive/_GameInspector.git
   ```

   Repository private cần Git credentials phù hợp. Với bản local, chọn **Add package from disk...** và chọn `package.json`.
2. Chọn một GameObject. Dưới header Inspector có mũi tên **GameInspector**, mặc định thu gọn. Bấm mũi tên hoặc chữ để mở.

## Chức năng

| Khu vực | Nút | Việc làm |
| --- | --- | --- |
| Toolbar | `Expand All` / `Collapse All` | Mở hoặc thu gọn mọi component của object |
| Prefab instance | `Apply Overrides` | Ghi override vào prefab gốc. Xám khi không có override |
| Prefab instance | `Revert` | Bỏ override, về giá trị prefab. Xám khi không có override |
| Object chưa là prefab | `Create Prefab` | Lưu thành prefab trong `Assets/Prefabs` và nối instance với prefab đó |
| Component | `Copy` | Copy component vào clipboard của Unity |
| Component | `Paste Values` | Dán giá trị clipboard vào component này (có Undo) |
| Component | `Remove` | Xoá component (có Undo). Khoá với `Transform` |
| Object | `Paste Component As New` | Thêm component mới từ clipboard |

Chỉ hoạt động khi chọn đúng một object. Toolbar không hiện cho model import bất biến. Với prefab asset hoặc nội dung trong Prefab Mode, một số nút prefab không hiện; component clipboard vẫn hiện.

## Mở rộng

Mỗi tính năng là một static class `Draw(GameObject)` trong `Editor/`. Thêm class mới rồi thêm một dòng vào `HeaderToolbar.OnHeader`.

## Giới hạn

- Nút component nằm dưới header GameObject, không nằm trong header từng component (cần reflection vào InspectorWindow, dễ vỡ khi đổi version Unity).
- Thư mục `Create Prefab` cố định `Assets/Prefabs` (hằng số trong `PrefabSection.cs`).
- `Remove` thất bại với log lỗi của Unity khi component khác đang yêu cầu nó (`RequireComponent`).

## Cấu trúc

```text
Editor/
  GameInspector.Editor.asmdef   assembly chỉ cho Editor
  HeaderToolbar.cs              điểm vào, foldout, Expand/Collapse
  PrefabSection.cs              Apply / Revert / Create Prefab
  ComponentSection.cs           Copy / Paste Values / Remove / Paste As New
```

## License

MIT, xem [LICENSE.md](LICENSE.md).
