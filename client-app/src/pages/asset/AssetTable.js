import React from "react";
import { TiDeleteOutline } from "react-icons/ti";
import { BsPencil } from "react-icons/bs";
import TableItem from "../../common/TableItem";
import NSTable from "../../common/NSTable";

const tableTitles = [
  {
    title: "Asset ID",
    nameSort: "sortCode",
  },
  {
    title: "Asset Name",
    nameSort: "sortName",
  },
  {
    title: "Category",
    nameSort: "sortCate",
  },
  {
    title: "State",
    nameSort: "sortState",
  },
];

export default function AssetTable({
  datas,
  totalPage,
  onChangePage,
  onChangeSort,
  onEdit,
  onDelete,
}) {
  const itemRender = (asset) => (
    <>
      <td>
        <TableItem>{asset.assetId}</TableItem>
      </td>
      <td>
        <TableItem>{asset.assetName}</TableItem>
      </td>
      <td>
        <TableItem>{asset.categoryName}</TableItem>
      </td>
      <td>
        <TableItem>{asset.state}</TableItem>
      </td>
      <td className="table-actions">
        <span className="table-icon" onClick={() => onEdit && onEdit(asset)}>
          <BsPencil color="#0d6efd" />
        </span>
        <span className="table-icon" onClick={() => onDelete && onDelete(asset)}>
          <TiDeleteOutline color="#dc3545" />
        </span>
      </td>
    </>
  );

  return (
    <NSTable
      titles={tableTitles}
      datas={datas}
      totalPages={totalPage}
      itemRender={itemRender}
      onChangeSort={onChangeSort}
      onChangePage={onChangePage}
    />
  );
}
