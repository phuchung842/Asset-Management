import { BsPencil } from "react-icons/bs";
import TableItem from "../../common/TableItem";
import { TiDeleteOutline, TiRefresh } from "react-icons/ti";
import NSTable from "../../common/NSTable";
import { assignmentOptions } from "../../enums/assignmentState";
import { formatDate } from "../../ultis/helper";

const tableTitles = [
  {
    title: "No",
    nameSort: "sortNumber",
  },
  {
    title: "Code",
    nameSort: "sortAssetId",
    width: "10%",
  },
  {
    title: "Asset Name",
    nameSort: "sortAssetName",
    width: "20%",
  },
  {
    title: "Assigned to",
    nameSort: "sortAssignedTo",
    width: "10%",
  },
  {
    title: "Assignedby",
    nameSort: "sortAssignedBy",
    width: "10%",
  },
  {
    title: "Asset Assigned Date",
    nameSort: "sortAssignedDate",
    width: "15%",
  },
  {
    title: "State",
    nameSort: "sortState",
  },
];
export default function AssignmentTable({
  datas,
  totalPage,
  onChangePage,
  onChangeSort,
  onEdit,
  onDeny,
  onReturn,
  pageSelected,
  onShowDetail,
}) {
  const itemRender = (assign) => (
    <>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>{assign.no}</TableItem>
      </td>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>{assign.assetId}</TableItem>
      </td>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>{assign.assetName}</TableItem>
      </td>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>{assign.assignedTo}</TableItem>
      </td>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>{assign.assignedBy}</TableItem>
      </td>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>{formatDate(assign.assignedDate)}</TableItem>
      </td>
      <td
        style={{ cursor: "pointer" }}
        onClick={() => onShowDetail && onShowDetail(assign)}
      >
        <TableItem>
          {assignmentOptions.find((item) => item.value === assign.state)
            ?.label ?? "Unknown"}
        </TableItem>
      </td>
      <td className="table-actions">
        <span className="table-icon">
          <BsPencil
            className={"border-0" + (assign.state === 2 ? "" : " disabled")}
            onClick={() => onEdit && onEdit(assign)}
          />
        </span>
        <span className="table-icon ns-text-primary">
          <TiDeleteOutline
            className={"border-0" + (assign.state === 2 ? " " : " disabled")}
            onClick={() => onDeny && onDeny(assign)}
          />
        </span>
        <span className="table-icon">
          <TiRefresh
            onClick={() => onReturn && onReturn(assign)}
            style={{
              color:
                assign.state === 2 || assign.state === 3 || assign.isReturning
                  ? ""
                  : " blue",
              fontSize: "1.3em",
            }}
            className={
              "border-0" +
              (assign.state === 2 || assign.state === 3 || assign.isReturning
                ? " disabled"
                : "")
            }
          />
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
      pageSelected={pageSelected}
    />
  );
}
