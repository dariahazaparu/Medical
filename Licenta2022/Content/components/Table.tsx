import { InputRef, Input, Space, Button, Dropdown, Menu } from 'antd';
import Table, { ColumnType } from 'antd/lib/table';
import { FilterConfirmProps } from 'antd/lib/table/interface';
import React, { useRef, useState } from 'react'
import { DownOutlined, EditOutlined, InfoOutlined, SearchOutlined } from '@ant-design/icons'
import Highlighter from 'react-highlight-words'

import get from 'lodash/get'
import { TableColumnsType } from './types';
import { addToRoute } from './utils';

export interface ITable {
    data: any[]
}

interface DataType {
    key: string;
    name: string;
    age: number;
    address: string;
}

type DataIndex = keyof DataType;


interface ITableComponent<T> {
    columns: TableColumnsType<T>
    data: T[]
}

const exampleColumns: TableColumnsType<DataType> = [
    {
        title: 'Name',
        dataIndex: 'name',
        key: 'name',
        width: '30%',
    },
    {
        title: 'Age',
        dataIndex: 'age',
        key: 'age',
        width: '20%',
    },
    {
        title: 'Address',
        dataIndex: 'address',
        key: 'address',
        sorter: (a, b) => a.address.length - b.address.length,
        sortDirections: ['descend', 'ascend'],
    },
];

const exampleData: DataType[] = [
    {
        key: '1',
        name: 'John Brown',
        age: 32,
        address: 'New York No. 1 Lake Park',
    },
    {
        key: '2',
        name: 'Joe Black',
        age: 42,
        address: 'London No. 1 Lake Park',
    },
    {
        key: '3',
        name: 'Jim Green',
        age: 32,
        address: 'Sidney No. 1 Lake Park',
    },
    {
        key: '4',
        name: 'Jim Red',
        age: 32,
        address: 'London No. 2 Lake Park',
    },
];

const TableComponent: React.FC<ITableComponent<any>> = ({ columns, data }) => {
    const [searchText, setSearchText] = useState('');
    const [searchedColumn, setSearchedColumn] = useState('');
    const searchInput = useRef<InputRef>(null);

    const handleSearch = (
        selectedKeys: string[],
        confirm: (param?: FilterConfirmProps) => void,
        dataIndex: DataIndex,
    ) => {
        confirm();
        setSearchText(selectedKeys[0]);
        setSearchedColumn(dataIndex);
    };

    const handleReset = (clearFilters: () => void) => {
        clearFilters();
        setSearchText('');
    };

    const getColumnSearchProps = (dataIndex: DataIndex): ColumnType<DataType> => ({
        filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }) => (
            <div style={{ padding: 8 }}>
                <Input
                    ref={searchInput}
                    placeholder={`Search ${dataIndex}`}
                    value={selectedKeys[0]}
                    onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
                    onPressEnter={() => handleSearch(selectedKeys as string[], confirm, dataIndex)}
                    style={{ marginBottom: 8, display: 'block' }}
                />
                <Space>
                    <Button
                        type="primary"
                        onClick={() => handleSearch(selectedKeys as string[], confirm, dataIndex)}
                        icon={<SearchOutlined />}
                        size="small"
                        style={{ width: 90 }}
                    >
                        Search
                    </Button>
                    <Button
                        onClick={() => clearFilters && handleReset(clearFilters)}
                        size="small"
                        style={{ width: 90 }}
                    >
                        Reset
                    </Button>
                    <Button
                        type="link"
                        size="small"
                        onClick={() => {
                            confirm({ closeDropdown: false });
                            setSearchText((selectedKeys as string[])[0]);
                            setSearchedColumn(dataIndex);
                        }}
                    >
                        Filter
                    </Button>
                </Space>
            </div>
        ),
        filterIcon: (filtered: boolean) => (
            <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />
        ),
        onFilter: (value, record) =>
            get(record, dataIndex)
                .toString()
                .toLowerCase()
                .includes((value as string).toLowerCase()),
        onFilterDropdownVisibleChange: visible => {
            if (visible) {
                setTimeout(() => searchInput.current?.select(), 100);
            }
        },
        render: text =>
            searchedColumn === dataIndex ? (
                <Highlighter
                    highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
                    searchWords={[searchText]}
                    autoEscape
                    textToHighlight={text ? text.toString() : ''}
                />
            ) : (
                text
            ),
    });

    const tableColumns = columns.map(column => ({
        ...((column as any).omitSearch ? {} : getColumnSearchProps((column as any).dataIndex)),
        ...column,
    })).concat(
        {
            title: 'Actiune',
            key: 'operation',
            fixed: 'right',
            width: 100,
            render: (data: any) => <Dropdown overlay={<Menu
                items={[
                    {
                        label: "Editare",
                        key: "edit",
                        icon: <EditOutlined />,

                        onClick: () => addToRoute(`/Edit/${data.Id}`)
                    },

                    {
                        label: "Detalii",
                        key: "details",
                        icon: <InfoOutlined />,

                        onClick: () => addToRoute(`/Details/${data.Id}`)
                    }
                ]}
            />}>
                <Button>
                    <Space>
                        Action
                        <DownOutlined />
                    </Space>
                </Button>
            </Dropdown>
        },
    )

    return <Table columns={tableColumns} dataSource={data} />;
};

export default TableComponent
