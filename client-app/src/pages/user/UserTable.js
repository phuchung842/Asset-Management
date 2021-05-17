import TableItem from '../../common/TableItem';
import NSTable from "../../common/NSTable";
import { BsPencil } from "react-icons/bs";
import { TiDeleteOutline } from "react-icons/ti";

const tableTitles = [
    {
        title: "Staff Code",
        nameSort: "sort StaffCode",
    },
    {
        title: "Full Name",
        nameSort: "sort FullName",
    },
    {
        title: "User Name",
        nameSort: "sort UserName",
    },
    {
        title: "Joined Date",
        nameSort: "sort JoinedDate",
    },
    {
        title: "Type",
        nameSort: "sort Type",
    },
]

function formatDate(date) {
    if (date == null) {
      date = Date.now();
    }
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();
  
    if (month.length < 2)
      month = '0' + month;
    if (day.length < 2)
      day = '0' + day;
    return [year, month, day].join('-');
  }

export default function UserTable({
    datas,
    totalPage,
    onChangePage,
    onChangeSort,
    onEdit,
    onDelete,
}) {
    const itemRender = (item) => (
        <>
            <td>
                <TableItem>{item.id}</TableItem>
            </td>
            <td>
                <TableItem>{item.userName}</TableItem>
            </td>
            <td>
                <TableItem>{item.fullName}</TableItem>
            </td>
            <td>
                <TableItem>{item.roleName}</TableItem>
            </td>
            <td>
                <TableItem>{formatDate(item.joinedDate)}</TableItem>
            </td>
            <td className="table-actions">
                <span className="table-icon" onClick={() => onEdit && onEdit(item)}>
                    <BsPencil color="#0d6efd" />
                </span>
                <span className="table-icon" onClick={() => onDelete && onDelete(item)}>
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