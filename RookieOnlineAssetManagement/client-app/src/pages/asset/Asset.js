import React from "react";
import AssetTable from "./AssetTable.js";
import { Row, Col, Table } from "reactstrap";
import { Link, useHistory } from "react-router-dom";
import { useNSModals } from "../../containers/ModalContainer.js";
import SearchBar from "../../common/SearchBar";
import CreateNew from "../../common/CreateNew";
import AssetFilterState from "./AssetFilterState.js";
import AssetFilterCategory from "./AssetFilterCategory";
import { formatDate, _createQuery } from "../../ultis/helper";
import http from "../../ultis/httpClient.js";
import NSConfirmModal, {
  useNSConfirmModal,
} from "../../common/NSConfirmModal.js";
import NSDetailModal, { useNSDetailModal } from "../../common/NSDetailModal";
import { PageContext } from "../../containers/PageLayout.js";

let params = {};

function _refreshParams() {
  params.sortCode = 0;
  params.sortName = 0;
  params.sortCate = 0;
  params.sortState = 0;
  params.page = 1;
}

export default function Asset(props) {
  const [assetDatas, setAssets] = React.useState(null);
  const [totalPages, setTotalPages] = React.useState(0);
  const [pageCurrent, setPageCurrent] = React.useState(0);
  const [itemDetail, setItemDetail] = React.useState(null);
  const [itemHistory, setItemHistory] = React.useState(null);
  const pageContext = React.useContext(PageContext);
  const history = useHistory();
  //modal
  const modalConfirm = useNSConfirmModal();
  const modalDetail = useNSDetailModal();
  const { modalAlert, modalLoading } = useNSModals();
  //handle
  React.useEffect(() => {
    params = {
      sortCode: 0,
      sortName: 0,
      sortCate: 0,
      sortState: 0,
      query: "",
      pagesize: 8,
      page: 1,
      state: [],
      categoryid: [],
      assetId: "",
    };
    _fetchData();
  }, []);

  const _fetchData = () => {
    pageContext?.payload ? (params.pagesize = 7) : (params.pagesize = 8);
    //
    http
      .get("/api/asset" + _createQuery(params))
      .then((resp) => {
        let val = resp.data;
        if (pageContext?.payload) {
          if (pageContext.payload.key === "asset") {
            val.unshift(pageContext?.payload.data);
            pageContext.setData(null);
          }
        }
        setAssets(val);
        let totalPages = resp.headers["total-pages"];
        setTotalPages(totalPages > 0 ? totalPages : 0);
        setPageCurrent(params.page);
      })
      .catch((err) => {
        setAssets([]);
      });
  };

  const handleChangePage = (page) => {
    // _refreshParams();
    params.page = page;
    _fetchData();
  };

  const handleChangeSort = (target) => {
    _refreshParams();
    params = { ...params, ...target };
    if (target < 0) return (params.sortCode = null);
    _fetchData();
  };

  const handleSearch = (query) => {
    _refreshParams();
    params.query = query;
    _fetchData();
  };

  const handleSearchKey = () => {
    _refreshParams();
    params.query = "";
    _fetchData();
  };

  const handleEdit = (item) => {
    history.push("/assets/" + item.assetId);
  };

  const handleDelete = (item) => {
    modalConfirm.config({
      message: "Do you want to delete this asset?",
      btnName: "Delete",
      onSubmit: (item) => {
        modalLoading.show();
        http
          .delete("/api/asset/" + item.assetId)
          .then((resp) => {
            _refreshParams();
            _fetchData();
          })
          .catch(showDisableDeleteModal)
          .finally(() => modalLoading.close());
      },
    });
    modalConfirm.show(item);
  };

  const handleFilterState = (items) => {
    _refreshParams();
    params.state = items;
    _fetchData();
  };

  const handleFilterCategory = (items) => {
    _refreshParams();
    params.categoryid = items;
    _fetchData();
  };

  const handleShowDetail = (item) => {
    params.assetId = item.assetId;
    Promise.all([
      http.get("/api/Asset/" + item.assetId),
      http.get("/api/Asset/history?assetId=" + item.assetId),
    ]).then((responseArray) => {
      setItemDetail(responseArray[0].data);
      setItemHistory(responseArray[1].data);
    });
    modalDetail.show();
  };

  const showDisableDeleteModal = (itemId) => {
    let msg = (
      <>
        Cannot delete the asset because it belongs to one or more historical
        assignments.If the asset is not able to be used anymore, please update
        its state in
        <Link to={"/asset/" + itemId}>To Edit Page</Link>
      </>
    );
    modalAlert.show({ title: "Can't delete asset", msg: msg });
  };

  return (
    <>
      <h5 className="name-list mb-4">Asset List</h5>
      <Row className="filter-bar mb-3">
        <Col xs={2}>
          <AssetFilterState onChange={handleFilterState} />
        </Col>
        <Col xs={2}>
          <AssetFilterCategory onChange={handleFilterCategory} />
        </Col>
        <Col>
          <SearchBar onSearch={handleSearch} onChangeKey={handleSearchKey} />
        </Col>
        <Col style={{ textAlign: "right" }}>
          <Link to="/new-asset">
            <CreateNew namecreate="Create new asset" />
          </Link>
        </Col>
      </Row>
      <AssetTable
        datas={assetDatas}
        onChangePage={handleChangePage}
        onChangeSort={handleChangeSort}
        onEdit={handleEdit}
        onDelete={handleDelete}
        totalPage={totalPages}
        pageSelected={pageCurrent}
        onShowDetail={handleShowDetail}
      />
      <NSConfirmModal hook={modalConfirm} />

      <NSDetailModal
        hook={modalDetail}
        title="Detailed Asset Information"
        size="lg"
      >
        <Table borderless className="table-detailed ">
          <tbody>
            <tr>
              <td>Asset Code : </td>
              <td>{itemDetail?.assetId}</td>
            </tr>
            <tr>
              <td>Asset Name : </td>
              <td>{itemDetail?.assetName}</td>
            </tr>
            <tr>
              <td>Category :</td>
              <td>{itemDetail?.categoryName}</td>
            </tr>

            <tr>
              <td>Installed Date : </td>
              <td>{formatDate(itemDetail?.installedDate)}</td>
            </tr>
            <tr>
              <td>State : </td>
              <td>{itemDetail?.state}</td>
            </tr>
            <tr>
              <td>Location : </td>
              <td>{itemDetail?.locationName}</td>
            </tr>
            <tr>
              <td>Specification :</td>
              <td>{itemDetail?.specification}</td>
            </tr>
            <tr>
              <td>History :</td>
              <Table>
                <thead>
                  <tr>
                    <th>Date</th>
                    <th>Assigned to</th>
                    <th>Assigned by</th>
                    <th>Returned date</th>
                  </tr>
                </thead>
                {itemHistory &&
                  itemHistory.map((item) => {
                    return (
                      <tbody>
                        <tr>
                          <td>{formatDate(item.date)}</td>
                          <td>{item.assignedTo}</td>
                          <td>{item.assignedBy}</td>
                          <td>{formatDate(item.returnedDate)}</td>
                        </tr>
                      </tbody>
                    );
                  })}
              </Table>
            </tr>
          </tbody>
        </Table>
      </NSDetailModal>
    </>
  );
}
