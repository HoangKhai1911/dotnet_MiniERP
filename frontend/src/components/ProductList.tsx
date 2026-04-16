import { useEffect, useState } from "react";
import api from "../api";

interface Product {
  id: number;
  sku: string;
  name: string;
  price: number;
  categoryName: string;
}

export default function ProductList() {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    api.get("/products").then(res => setProducts(res.data));
  }, []);

  return (
    <div>
      <h3>Danh sách sản phẩm</h3>
      <table border={1} cellPadding={5}>
        <thead>
          <tr>
            <th>SKU</th>
            <th>Tên</th>
            <th>Danh mục</th>
            <th>Giá</th>
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.id}>
              <td>{p.sku}</td>
              <td>{p.name}</td>
              <td>{p.categoryName}</td>
              <td>{p.price}₫</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
