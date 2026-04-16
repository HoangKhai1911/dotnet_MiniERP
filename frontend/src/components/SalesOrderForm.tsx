import React, { useEffect, useState } from 'react';
import { Form, Select, InputNumber, Button, Space, Card, Divider, message, Table } from 'antd';
import { MinusCircleOutlined, PlusOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import api from '../api';

interface SalesOrderFormProps {
  onSuccess?: () => void;
}

const SalesOrderForm: React.FC<SalesOrderFormProps> = ({ onSuccess }) => {
  const [form] = Form.useForm();
  const [products, setProducts] = useState<any[]>([]);
  const [customers, setCustomers] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  // Lấy dữ liệu sản phẩm và khách hàng để đổ vào Select
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [prodRes, custRes] = await Promise.all([
          api.get('/Products'),
          api.get('/Customers')
        ]);
        setProducts(prodRes.data);
        setCustomers(custRes.data);
      } catch (error) {
        message.error('Không thể tải dữ liệu bổ trợ');
      }
    };
    fetchData();
  }, []);

  // Hàm xử lý khi nhấn Lưu đơn hàng
 const onFinish = async (values: any) => {
  setLoading(true);
  try {
    // Payload phải khớp chính xác với SalesOrderCreateDto.cs
    const payload = {
      customerId: values.customerId,
      items: values.items.map((item: any) => {
        // Tìm sản phẩm để lấy đơn giá nếu người dùng không nhập
        const selectedProduct = products.find(p => p.id === item.productId);
        return {
          productId: item.productId,
          quantity: item.quantity,
          unitPrice: selectedProduct?.price || 0 
        };
      })
    };

    await api.post('/SalesOrders', payload);
    message.success('Tạo đơn hàng thành công!');
    form.resetFields();
    if (onSuccess) onSuccess();
  } catch (error: any) {
    // Hiển thị lỗi từ Validation của C# (như [Required] hay [Range])
    const errorMsg = error.response?.data?.errors 
      ? "Dữ liệu không hợp lệ, vui lòng kiểm tra lại"
      : "Lỗi kết nối server";
    message.error(errorMsg);
  } finally {
    setLoading(false);
  }
};

  return (
    <Card title={<span><ShoppingCartOutlined /> Tạo đơn hàng mới</span>} bordered={false}>
      <Form form={form} layout="vertical" onFinish={onFinish}>
        {/* Chọn khách hàng */}
        <Form.Item name="customerId" label="Khách hàng" rules={[{ required: true, message: 'Vui lòng chọn khách hàng' }]}>
          <Select placeholder="Chọn khách hàng" showSearch optionFilterProp="children">
            {customers.map(c => <Select.Option key={c.id} value={c.id}>{c.name}</Select.Option>)}
          </Select>
        </Form.Item>

        <Divider orientation={"left" as any}>Chi tiết sản phẩm</Divider>

        {/* Dynamic Form cho Items */}
        <Form.List name="items">
          {(fields, { add, remove }) => (
            <>
              {fields.map(({ key, name, ...restField }) => (
                <Space key={key} style={{ display: 'flex', marginBottom: 8 }} align="baseline">
                  <Form.Item
                    {...restField}
                    name={[name, 'productId']}
                    rules={[{ required: true, message: 'Chọn SP' }]}
                    style={{ width: 300 }}
                  >
                    <Select 
  placeholder="Chọn sản phẩm" 
  showSearch 
  optionFilterProp="children"
  // Thêm sự kiện onChange để tự động điền đơn giá nếu cần
  onChange={(value) => {
    const selectedProd = products.find(p => p.id === value);
    console.log("Giá sản phẩm này là:", selectedProd?.price);
  }}
>
  {products.map(p => (
    <Select.Option key={p.id} value={p.id}>
      <div style={{ display: 'flex', justifyContent: 'space-between' }}>
        <span>{p.name}</span>
        <span style={{ color: '#999' }}>{p.price?.toLocaleString()} ₫</span>
      </div>
    </Select.Option>
  ))}
</Select>
                  </Form.Item>

                  <Form.Item
                    {...restField}
                    name={[name, 'quantity']}
                    rules={[{ required: true, message: 'Số lượng' }]}
                  >
                    <InputNumber min={1} placeholder="SL" />
                  </Form.Item>

                  <MinusCircleOutlined onClick={() => remove(name)} style={{ color: 'red' }} />
                </Space>
              ))}
              <Form.Item>
                <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                  Thêm sản phẩm vào đơn
                </Button>
              </Form.Item>
            </>
          )}
        </Form.List>

        <Form.Item>
          <Button type="primary" htmlType="submit" loading={loading} block size="large">
            XÁC NHẬN LƯU ĐƠN HÀNG
          </Button>
        </Form.Item>
      </Form>
    </Card>
  );
};

export default SalesOrderForm;