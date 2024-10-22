Linh: Code ItemAmmo để player nhăt và cộng thêm vào tổng số đạn hiện tại (Có hai loại đạn là Đạn thường và Lựu đạn)

Lâm: Code ItemFuel để player nhặt và cộng thêm vào số nhiên liệu hiện tại (Có hai loại là loại nhiên liệu nhỏ và lớn)

Duy: Code ItemHealth để player đi đến nhặt và hồi máu

Cả 3 lớp trên đều kế thừa từ Item

Cách code item cơ bản:
1. Tạo một thực thể 3D trong Scene:

    1.1. Di chuột vào tab Hierachy -> Chuột trái -> 3D Object -> Chọn bất kì mô hình 3D nào (Chưa cần model chính xác cho item đó)

   1.2. Lúc này sẽ có một thực thể mô hình 3D xuất hiện trong tab Scene, nếu không tìm thấy nó thì vào tab Hierachy chọn mô hình vừa tạo và ấn F để focus vào nó

   1.3. Bắt đầu thêm các component (những khối logic thêm vào cho GameObject, có thể do Unity tạo hoặc do chúng ta tự tạo, nó chính là viết script) cần thiết vào mô hình trên

    1.4. Vì Item chỉ cần phát hiện va chạm với người chơi và đưa loại Item cho họ nên ta chỉ cần thêm component Collider và component logic chúng ta thêm vào dựa vào các loại item trên

   1.5. Chọn thực thể item vừa tạo và vào tab Inspector, chọn Add Component ở dưới, gõ collider và chọn loại collider phù hợp (chỉ khác về hình dạng), không chọn collider 2d

    1.6. Item sẽ không cần va chạm vật lý với người chơi, tức người chơi sẽ đi xuyên qua Item và nhận nó -> Chọn Collider và tick Is Trigger

3. Lớp cha Item trong Scripts/Item/Item.cs đã có hai phương thức chính là:
   OnTriggerEnter(Collider other): Phát hiện nếu đối tượng va chạm là other có tag là "Player" thì Item sẽ lưu lại đối tượng Player vào playerObject, thực hiện hàm giveItemToPlayer() và tự huỷ chính nó
   giveItemToPlayer(): Đây chính là hàm để các ông viết thêm logic mong muốn cho loại item mà các ông code.
   
4. Tạo script loại item cần code (ItemHealth | ItemAmmo | ItemFuel), kế thừa từ class Item trên và code

5. Sau khi code xong, chọn thực thể 3D tạo ở trên, lấy script vừa code đó và kéo vào tab Inspector của thực thể đó để thêm component

6. Chạy thử để test logic
